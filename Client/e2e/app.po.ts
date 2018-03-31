import { browser, element, by } from 'protractor';

export class ClientPage {
  navigateTo() {
    return browser.get('/');
  }

  getPageText() {
    return element(by.id('private-nav')).getText();
  }
}
