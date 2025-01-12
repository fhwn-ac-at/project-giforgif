import { HttpErrorResponse } from '@angular/common/http';
import { throwError } from 'rxjs';

export function handleError(error: HttpErrorResponse) {
  const errorBody = error.error;

  return throwError(() => errorBody);
}