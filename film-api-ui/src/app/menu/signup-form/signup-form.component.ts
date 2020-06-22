import { Component } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { User } from '../user';
import { FormGroup, FormControl, Validators, ValidatorFn, AbstractControl } from '@angular/forms';

@Component({
  selector: 'app-signup-form',
  templateUrl: './signup-form.component.html',
  styleUrls: ['./signup-form.component.scss']
})
export class SignupFormComponent {

  hide: boolean = true;
  
  profileForm = new FormGroup({
    name: new FormControl(''),
    surname: new FormControl(''),
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('',[Validators.required,this.passwordValidator])
  });

  private passwordValidator (control: AbstractControl): { [key: string]: string | null} {
      let passwd = control.value;
      const condition = passwd.length > 8 
      && passwd.match(/[A-Z]/) 
      && passwd.match(/[a-z]/)
      && passwd.match(/[0-9]/);
      return condition ? null : { 'badPassword': control.value };
  }

  getPasswdError() {
    if (this.profileForm.controls.password.hasError('required'))
      return 'You must enter a password';
    if ( this.profileForm.controls.password.hasError('badPassword') )
    return 'Should be at least 8 charactes, 1 lowercase,  1 uppercase and 1 digit';
    }

  getEmailError() {
    let email = this.profileForm.controls.email;
    if (email.hasError('required')) {
      return 'You must enter a value';
    }
    return email.hasError('email') ? 'Not a valid email' : '';
  }

  constructor(
    public dialogRef: MatDialogRef<SignupFormComponent>) {}

  onSubmit() {
    if ( this.profileForm.valid ) {
      let user: User = this.profileForm.value;
      console.log(user);
      this.dialogRef.close(user);  
    }
  }

  onCancel(): void {
    this.dialogRef.close();
  }

}
