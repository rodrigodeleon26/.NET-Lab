import { Component } from '@angular/core';

@Component({
  selector: 'app-copagos',
  templateUrl: './copagos.component.html',
  styleUrl: './copagos.component.css'
})
export class CopagosComponent {
  loading: boolean = false;
  errorMessage: string = '';
  successMessage: string = '';


  showSuccessMessage(message: string) {
    this.successMessage = message;
      setTimeout(() => {
        this.successMessage = '';
      }, 3000);
  }

  showErrorMessage(message: string) {
    this.errorMessage = message;
    setTimeout(() => {
      this.errorMessage = '';
    }, 3000);
  }
}
