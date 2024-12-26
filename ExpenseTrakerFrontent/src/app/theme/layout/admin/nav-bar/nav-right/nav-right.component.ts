// Angular import
import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AppNotification, AuthService, CommonService, SignalRService, ToastService, TokenService } from 'src/services';

@Component({
  selector: 'app-nav-right',
  templateUrl: './nav-right.component.html',
  styleUrls: ['./nav-right.component.scss']
})
export class NavRightComponent {
  userName:any;
  userId:any;
  profileImage: any = [];
  notifications: AppNotification[] = [];
  unreadNotificationCount: number = 0;
 constructor(private _apiService:AuthService,
  private tokenService:TokenService,
  private toast:ToastService,
  private route: Router,
  private commonservice:CommonService,
  public signalRService: SignalRService
 ){}

 ngOnInit(){
 this.userName=this.tokenService?.getUserDetails().UserName;
  this.userId=this.tokenService?.getUserDetails().UserID;
  this.getUserProfileImage();
  this.signalRService.getNotificationsSubject().subscribe((data: AppNotification[]) => {
    this.notifications = data;
  });

  this.fetchNotifications();
 }

 logOut(): void {
  this._apiService.logoutUser().subscribe({
    next: (response) => {
      if (response['success']) {
        this.tokenService.clearAuthToken();
        this.tokenService.clearStorage();
        this.toast.showSuccess(response.message);
        this.route.navigateByUrl("/login/login");
      }
    },
    error: (errorMessage) => {
      this.toast.showError(errorMessage);
    },
  });
}
 
  getUserProfileImage(){
    this.commonservice.getUserProfileImage(this.userId).subscribe({
      next:(response)=>{
        const userData = response.data;
          this.profileImage = userData.images;
      },
      error:(errorMessage)=>{
        this.toast.showError(errorMessage);
      }
    })
  }


profilePictureSave(event: any) {
  var numberOfFiles: number = event.target.files.length;

  if (numberOfFiles > 0) {
    if (
      event.target.files[0].type == "image/jpeg" ||
      event.target.files[0].type == "image/jpg" ||
      event.target.files[0].type == "image/png"
    ) {
      const userId = parseInt(this.userId);
      if (userId > 0) {
        let form = new FormData();

        form.append("UserId", userId.toString());
        form.append("Images", event.target.files[0].name);
        form.append("ProfileImage", event.target.files[0]);

        this.commonservice
          .saveProfilePicture(form)
          .pipe()
          .subscribe({
            next: (response) => {
              if (response['success']) {
                this.toast.showSuccess(response.message);
                 this.getUserProfileImage();
              } else {
                this.toast.showWarn(response.message);
              }
            },
            error: (error) => {
              this.toast.showError(error);
            },
          });
      }
    } else {
      this.toast.showWarn("Please upload only JPEG, JPG or PNG files");
    }
  }
}
fetchNotifications() {
  this.signalRService.getNotifications(this.userId, false).subscribe((data: AppNotification[]) => {
    this.notifications = data;
    this.unreadNotificationCount = this.notifications.filter(n => !n.isRead).length; 
  });
}

markAsRead(notification: AppNotification) {
  notification.isRead = true;
  this.signalRService.getNotifications(notification.userId, true).subscribe(() => {
    this.unreadNotificationCount = this.notifications.filter(n => !n.isRead).length;
  });
}

markAllAsRead() {
  this.notifications.forEach((n) => (n.isRead = true));
  this.unreadNotificationCount = 0; 
  this.signalRService.getNotifications(this.userId, true).subscribe(() => {
    this.fetchNotifications(); 
  });
}

isNotificationRead(notification: AppNotification): boolean {
  return notification.isRead;
}

}