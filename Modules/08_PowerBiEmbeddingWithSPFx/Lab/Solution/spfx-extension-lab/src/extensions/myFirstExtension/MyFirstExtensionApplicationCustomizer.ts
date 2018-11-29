import { override } from '@microsoft/decorators';

import {
  BaseApplicationCustomizer,
  PlaceholderContent,
  PlaceholderName
} from '@microsoft/sp-application-base';

import styles from './MyApplicationCustomizerStyles.module.scss'

export default class MyFirstExtensionApplicationCustomizer
  extends BaseApplicationCustomizer<any> {

  private PageHeader: PlaceholderContent | undefined;
  private PageFooter: PlaceholderContent | undefined;


  private RenderPlaceHolders(): void {

    if (!this.PageHeader) {
      this.PageHeader = this.context.placeholderProvider.tryCreateContent(PlaceholderName.Top);
      if (!this.PageHeader) {
        console.error('The expected placeholder (Top) was not found.');
        return;
      }
      this.PageHeader.domElement.innerHTML = `
    <div class="${styles.app}">
      <div class="${styles.top}">
        <div>This is the page header</div>
      </div>
    </div>`;
    }

    if (!this.PageFooter) {
      this.PageFooter = this.context.placeholderProvider.tryCreateContent(PlaceholderName.Bottom);
      if (!this.PageFooter) {
        console.error('The expected placeholder (Bottom) was not found.');
        return;
      }
      this.PageFooter.domElement.innerHTML = `
    <div class="${styles.app}">
      <div class="${styles.bottom}">
        <div>This is the page footer</div>
      </div>
    </div>`;
    }

  }

  @override
  public onInit(): Promise<void> {
    this.context.placeholderProvider.changedEvent.add(this, this.RenderPlaceHolders);
    this.RenderPlaceHolders();
    return Promise.resolve<void>();
  }


}
