import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { Toast } from '../../types/toast/toast';

@Injectable({
  providedIn: 'root'
})
export class ToastService {
  private toastId = 0;
  private toasts: Toast[] = [];
  private toastsSubject = new Subject<Toast[]>();

  public toasts$ = this.toastsSubject.asObservable();

  constructor() {}

  public success(message: string) {
    this.addToast(message, 'Erfolgreich', false);
  }

  public error(message: string) {
    this.addToast(message, 'Fehler', true);
  }

  private addToast(message: string, heading: string, error: boolean = false) {
    const toast: Toast = { error, message, id: this.toastId++, heading };
    this.toasts.push(toast);
    this.toastsSubject.next(this.toasts);

    setTimeout(() => {
      this.removeToast(toast.id);
    }, 3000);
  }

  private removeToast(id: number) {
    this.toasts = this.toasts.filter((toast) => toast.id !== id);
    this.toastsSubject.next(this.toasts);
  }
}
