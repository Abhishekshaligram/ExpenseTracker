// toast.service.ts
import { Injectable } from "@angular/core";
import { MessageService } from "primeng/api";

@Injectable({
  providedIn: "root",
})
export class ToastService {
  constructor(private messageService: MessageService) {}

  showSuccess(detail: string) {
    this.messageService.add({
      severity: "success",
      summary: "Success",
      detail: detail,
    });
  }

  showError(detail: string) {
    this.messageService.add({
      severity: "error",
      summary: "Error",
      detail: detail,
    });
  }

  showInfo(detail: string) {
    this.messageService.add({
      severity: "info",
      summary: "info",
      detail: detail,
    });
  }

  showWarn(detail: string) {
    this.messageService.add({
      severity: "warn",
      summary: "warn",
      detail: detail,
    });
  }
}
