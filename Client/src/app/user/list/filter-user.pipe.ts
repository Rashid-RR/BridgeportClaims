import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'filterUser'
})
export class FilterUserPipe implements PipeTransform {

  transform(users: any, searchText: any, isAdmin: any): any {

    if (searchText == null && (isAdmin == null || !isAdmin)) { return users; }
    return users.filter((user) => {
      if (isAdmin && searchText == null) {
        return (user.admin);
      } else if (isAdmin && searchText != null) {
        return (user.admin) && (user.firstName.toLowerCase().includes(searchText.toLowerCase()) ||
          user.lastName.toLowerCase().includes(searchText.toLowerCase()));
      } else {
        return user.firstName.toLowerCase().includes(searchText.toLowerCase()) ||
          user.lastName.toLowerCase().includes(searchText.toLowerCase());
      }
    });
  }
}
