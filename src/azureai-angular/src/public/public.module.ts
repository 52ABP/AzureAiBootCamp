import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PublicRoutingModule } from './public-routing.module';
import { LayoutComponent } from './layout/layout.component';
import { AzureAiBootCampComponent } from './azure-ai-boot-camp/azure-ai-boot-camp.component';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '@shared/shared.module';
import { AbpModule } from 'yoyo-ng-module/src/abp';
import { HttpModule, JsonpModule } from '@angular/http';
import { NgZorroAntdModule } from 'ng-zorro-antd';
import { ServiceProxyModule } from '@shared/service-proxies/service-proxy.module';
import { AzureOcrComponent } from './azure-ai-boot-camp/components/azure-ocr/azure-ocr.component';
import { AzureSceneRecognitionComponent } from './azure-ai-boot-camp/components/azure-scene-recognition/azure-scene-recognition.component';
import { AzureTextToSpeechComponent } from './azure-ai-boot-camp/components/azure-text-to-speech/azure-text-to-speech.component';
import { AzureSpeechToTextComponent } from './azure-ai-boot-camp/components/azure-speech-to-text/azure-speech-to-text.component';

const COMPONENTS = [
  LayoutComponent,
  AzureAiBootCampComponent,
  AzureSceneRecognitionComponent,
  AzureOcrComponent,
  AzureTextToSpeechComponent
]


@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    HttpModule,
    JsonpModule,
    NgZorroAntdModule,
    AbpModule,
    SharedModule,
    ServiceProxyModule,
    PublicRoutingModule,
  ],
  declarations: [
    ...COMPONENTS,
    AzureSpeechToTextComponent,
  ],
  entryComponents: [

  ],
  providers: [

  ]
})
export class PublicModule { }
