import { Component, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/component-base';
import { HttpClient } from '@angular/common/http';
import { AzureServiceProxy } from '@shared/service-proxies/service-proxies';

@Component({
  selector: 'app-azure-ai-boot-camp',
  templateUrl: './azure-ai-boot-camp.component.html',
  styleUrls: [
    './azure-ai-boot-camp.component.less'
  ]
})
export class AzureAiBootCampComponent extends AppComponentBase
  implements OnInit {

  isLoding: boolean;
  langs = {
    ocr: [],
    speechToText: [],
    textToSpeech: [],
  }


  constructor(
    injector: Injector,
    private httpClient: HttpClient,
    private _azureService: AzureServiceProxy,
  ) {
    super(injector);

  }
  ngOnInit() {
    this.isLoding = true;
    this.httpClient.get("/assets/azureLang.json")
      .finally(() => {
        this.isLoding = false;
      })
      .subscribe((result) => {
        let res = <any>result;
        this.langs.ocr = res.ocr;
        this.langs.speechToText = res.speechToText;
        this.langs.textToSpeech = res.textToSpeech;

      });
  }


}