import { Observable, catchError, finalize, take } from 'rxjs';
import { inject, signal } from '@angular/core';

import { HttpErrorResponse } from '@angular/common/http';
import { ToastService } from '../services/toast/toast.service';
import { handleError } from '../pipes/handleError.pipe';

export class Perform<T> {
  public data = signal<T | undefined>(undefined);
  public isLoading = false;
  public hasError = false;
  public errors: HttpErrorResponse[] = [];

  private action$: Observable<T> | undefined;

  private messageService;

  constructor(messageService?: ToastService) {
    this.messageService = messageService ?? inject(ToastService);
  }

  public load(
    action$: Observable<T>,
    options?: {
      toast?: {
        success?: string;
        info?: string;
        error?: string;
      };
      touch?: (response: T) => Promise<void>;
      mutate?: (response: T) => Promise<T>;
      error?: (error: HttpErrorResponse) => Promise<void>;
    }
  ) {
    this.isLoading = true;
    this.hasError = false;
    this.action$ = action$;

    this.action$
      .pipe(
        take(1),
        catchError(handleError),
        finalize(() => {
          this.isLoading = false;
        })
      )
      .subscribe({
        next: async (response: T) => {
          this.isLoading = false;
          if (options?.toast?.success) {
            this.messageService.success(options.toast.success);
          }

          if (options?.toast?.info) {
            this.messageService.success(options.toast.info);
          }

          this.data.set(response);

          if (options?.touch) {
            await options.touch(response);
          }

          this.data.set(
            options?.mutate ? await options.mutate(response) : response
          );
        },
        error: async (error: HttpErrorResponse) => {
          this.hasError = true;
          this.isLoading = false;

          if (options?.error) {
            await options.error(error);
          }

          if (options?.toast?.error) {
            this.messageService.error(options.toast.error);
          }
        },
      });
  }
}
