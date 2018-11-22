import { browser } from 'protractor';
import { ClientPage } from './app.po';
import { ClaimsPage } from './claims-app.po';

describe('client App', () => {
  let page: ClientPage;
  let claimPage: ClaimsPage;

  beforeEach(() => {
    page = new ClientPage();
    claimPage = new ClaimsPage();
  });

  it('App should navigate to login send test credentials then login', () => {
    //page.navigateTo();
    //expect(page.getPageText()).toContain('Claims!');
    claimPage.navigateToLogin();
    claimPage.login();
    claimPage.loadClaims();
    claimPage.loadClaim();
    claimPage.checkClaimLoaded();
  });
  afterEach(function () {
    var cls=console;
    browser.manage().logs().get('browser').then(function (browserLog) {
      browserLog.forEach(log=>{
        if(log.level.name=='ERROR'){
          cls.log('Got an error '+log.message);
        }
        expect(log.level.name).not.toEqual('ERROR')
      });
    });
  });
});
