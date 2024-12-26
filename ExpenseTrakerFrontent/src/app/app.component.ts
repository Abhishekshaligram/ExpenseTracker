import { Component } from '@angular/core';
import { SpinnerService } from 'src/services';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  isLoading: boolean = false;
  title = 'Berry Angular Free Version';
  constructor(private spinnerservice:SpinnerService){
    this.spinnerservice.spinner$.subscribe((isLoading: boolean) => {
      setTimeout(() => {
        this.isLoading = isLoading;
      });
    });
  }
}
