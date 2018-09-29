
export class User {

  constructor(id?: string, userName?: string, fullName?: string, email?: string, roles?: string[]) {

    this.id = id;
    this.userName = userName;
    this.fullName = fullName;
    this.email = email;
    this.roles = roles;
  }


  get friendlyName(): string {
    let name = this.fullName || this.userName;
    return name;
  }


  public id: string;
  public userName: string;
  public fullName: string;
  public email: string;
  public isEnabled: boolean;
  public isLockedOut: boolean;
  public roles: string[];
}
