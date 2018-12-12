import { Component, OnInit, Injector, Input } from '@angular/core';
import { AppComponentBase } from '@shared/component-base';
import { AzureServiceProxy } from '@shared/service-proxies/service-proxies';

@Component({
  selector: 'app-azure-ocr',
  templateUrl: './azure-ocr.component.html',
  styleUrls: [
    './azure-ocr.component.less'
  ]
})
export class AzureOcrComponent extends AppComponentBase
  implements OnInit {

  _langs = [];
  @Input()
  set langs(value: any[]) {
    this._langs = value;
    if (this._langs && this._langs.length > 0) {
      this.requestParms.lang = this._langs[0].value;
    }
  }

  isLoding: boolean;

  // OCR识别使用的属性
  requestParms = {
    imgUrl: "https://leizhangstorage.blob.core.chinacloudapi.cn/azureblog/ocr.jpg",
    lang: null
  }
  result: string;


  constructor(
    injector: Injector,
    private _azureService: AzureServiceProxy,
  ) {
    super(injector);

  }
  ngOnInit() {

  }

  /**
 * ocr分析
 */
  ocrAnalyze() {
    this.isLoding = true;
    this.result = null;
    this._azureService.ocr(this.requestParms.imgUrl, this.requestParms.lang)
      .finally(() => {
        this.isLoding = false;
      })
      .subscribe((result) => {
        this.result = result;
      })
  }
}
