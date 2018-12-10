import { Component, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/component-base';

@Component({
  selector: 'app-azure-ai-boot-camp',
  templateUrl: './azure-ai-boot-camp.component.html',
  styleUrls: [
    './azure-ai-boot-camp.component.less'
  ]
})
export class AzureAiBootCampComponent extends AppComponentBase
  implements OnInit {
  ocrImgUrl: string;

  ocrLoding: boolean = false;

  constructor(
    injector: Injector,
  ) {
    super(injector);
  }
  ngOnInit() {
    this.ocrImgUrl = "https://leizhangstorage.blob.core.chinacloudapi.cn/azureblog/ocr.jpg";
  }
  ocrAnalyze() {
    this.ocrLoding = true;
    const self = this;

    setTimeout(() => {
      self.ocrLoding = false;
    }, 3000);

  }


}