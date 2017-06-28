import {Component, OnInit} from "@angular/core";
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {Router} from "@angular/router";
import {HttpService} from "../../services/http-service"
import {ClaimManager} from "../../services/claim-manager";
import {UserProfile} from "../../models/profile";
import {ProfileManager} from "../../services/profile-manager";
import {warn,success} from "../../models/notification"

@Component({
  selector: 'app-profile',
  templateUrl: "profile.component.html",
  //styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  form: FormGroup;
  submitted: boolean = false;
  registered: boolean = false;
  emailRegex = '^[A-Za-z0-9]+(\.[_A-Za-z0-9]+)*@[A-Za-z0-9-]+(\.[A-Za-z0-9-]+)*(\.[A-Za-z]{2,15})$';
  saveLogin:boolean=false;
  saveEmail:boolean=false;
  loginError:string='';
  emailError:string='';
  constructor(private formBuilder: FormBuilder,public claimManager:ClaimManager,private http:HttpService,private profileManager:ProfileManager) {
    if(this.profileManager.profile==null){
      this.profileManager.profile = new UserProfile('','','','','');
    }
    this.form = this.formBuilder.group({
      oldPassword: ['', Validators.compose([Validators.required])],
      newPassword: ["", Validators.compose([Validators.required])],
      confirmPassword: ["", Validators.compose([Validators.required])]
    });
  }

  ngOnInit() {
     
  }

  updatePassword() {
    this.submitted = true;
    if (this.form.valid && this.form.get('newPassword').value !== this.form.get('confirmPassword').value) {
      this.form.get('confirmPassword').setErrors({"unmatched": "Confirm password does not match password"});
    }
    if (this.form.valid) {
      try {
        this.http.changepassword(this.form.value).subscribe(res => {
            success('Password successfully changed');
            this.registered = true
        },error=>{
             let err = error.json();
             warn( err.Message);
        })
      } catch (e) {
       warn('Error in fields. Please correct to proceed!');

      }
    }else{
       warn('Error in fields. Please correct to proceed!');
    }
  }
}
