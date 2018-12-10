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

const COMPONENTS = [
  LayoutComponent,
  AzureAiBootCampComponent
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
  declarations: [...COMPONENTS],
  entryComponents: [

  ],
  providers: [

  ]
})
export class PublicModule { }
