import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { AlertifyService } from '../_services/alertify.service';
import { FormGroup, FormControl, Validators, ValidationErrors, FormBuilder } from '@angular/forms';
import { CustomValidators } from '../Validator/custome-validators';
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { User } from '../Models/user';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Input() valuesFromHome: any;
  @Output() cancelRegsiter = new EventEmitter();
  registerForm: FormGroup;
  bsConfigs: Partial<BsDatepickerConfig>;
  model: User;
  constructor( private router: Router, private fb: FormBuilder, private authService: AuthService, private alertify: AlertifyService) { }

  ngOnInit() {
    this.bsConfigs = Object.assign({}, { containerClass: 'theme-red' });
    this.createREgisterForm();
  }
  createREgisterForm() {
    this.registerForm = this.fb.group(
      {
      gender: ['male'],
      username:  [null , Validators.required],
      password: [null , Validators.compose([
          Validators.required,
          // check whether the entered password has a number
          CustomValidators.patternValidator(/\d/, {
            hasNumber: true
          }),
          // check whether the entered password has upper case letter
          CustomValidators.patternValidator(/[A-Z]/, {
            hasCapitalCase: true
          }),
          // check whether the entered password has a lower case letter
          CustomValidators.patternValidator(/[a-z]/, {
            hasSmallCase: true
          }),
          // check whether the entered password has a special character
          CustomValidators.patternValidator(
            /[ !@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]/,
            {
              hasSpecialCharacters: true
            }
          ),
          Validators.minLength(8)
        ])]
      ,
      knownAs: ['', Validators.required],
      dateOfBirth: [null, Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      confirmPassword: ['', Validators.required]
      }, {
        // check whether our password and confirm password match
        validator: CustomValidators.passwordMatchValidator
      });
  }
  register() {
    if (this.registerForm.valid) {
      this.model = Object.assign({}, this.registerForm.value);
    this.authService.register(this.model).subscribe(_ => {
      this.alertify.success('Regstriation done successfuly');
      this.authService.changeMemberPhoto('../../assets/user.png');
    }, error => {
     this.alertify.error(error);
    }, () => {
      this.authService.login(this.model).subscribe(next => {
        this.alertify.success('Logged In Successfully');
      }, (error) => {
        this.alertify.error(error);
      }, () => {
        this.router.navigate(['/members']);

      });
    });
}
  }

  calcel() {
    this.cancelRegsiter.emit(false);

  }
  passwordMatch(g: FormGroup) {
    return g.get('password').value === g.get('confirmedpassword').value ? null : {'mismatch': true};
  }
}
