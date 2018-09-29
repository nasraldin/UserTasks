// ====================================================
// More Templates: https://www.ebenmonney.com/templates
// Email: support@ebenmonney.com
// ====================================================

import { AppPage } from './app.po';

describe('UserTasks App', () => {
  let page: AppPage;

  beforeEach(() => {
    page = new AppPage();
  });

  it('should display application title: UserTasks', () => {
    page.navigateTo();
    expect(page.getAppTitle()).toEqual('UserTasks');
  });
});
