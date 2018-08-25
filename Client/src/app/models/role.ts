export class Role {
    id: String;
    name: String;
    users: any;
    constructor(id: String, name: String, users: any) {
        this.id = id;
        this.name = name;
        this.users = users;
    }
}
