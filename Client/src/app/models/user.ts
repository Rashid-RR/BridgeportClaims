export class User {

        url: String;
        id: String;
        userName: String;
        firstName: String;
        lastName: String
        fullName: String;
        registeredDate: Date;
        email: String;
        emailConfirmed: Boolean;
        roles: String;
        admin:boolean;
        user:boolean;
        claims: any;
    constructor(url: String,id: String,
            userName: String,
            firstName: String,
            lastName: String, 
            fullName: String, 
            registeredDate: Date,
            email: String,
            emailConfirmed: Boolean,
            roles:String,
            admin:boolean,
            user:boolean,
            claims:any) {
        this.url = url; 
        this.userName = userName;
        this.firstName = firstName;
        this.lastName = lastName;
        this.fullName = fullName;
        this.email = email;
        this.emailConfirmed = emailConfirmed;
        this.roles = roles;
        this.admin = admin
        this.user = user;
        this.claims = claims;

    }
}
