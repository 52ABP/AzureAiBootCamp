import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LayoutComponent } from './layout/layout.component';
import { AzureAiBootCampComponent } from './azure-ai-boot-camp/azure-ai-boot-camp.component';

const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    children: [
      {
        path: 'azure-ai-boot-camp',
        component: AzureAiBootCampComponent,
      },
      {
        path: '**',
        redirectTo: 'azure-ai-boot-camp',
      },
    ],
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PublicRoutingModule { }
