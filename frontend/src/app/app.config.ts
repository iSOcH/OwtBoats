import {ApplicationConfig, importProvidersFrom, provideZoneChangeDetection} from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import {provideHttpClient} from '@angular/common/http';
import { ApiModule } from './api/api.module';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(),

    // this should be easier, but until the NgModule related stuff is updated, we use the solution provided in https://github.com/cyclosproject/ng-openapi-gen/issues/336#issue-2704985424
    // we need to set the url manually since it is defaulted with a trailing /, causing requests to go to localhost:5261//auth/... which will always 404
    importProvidersFrom(ApiModule.forRoot({ rootUrl: "http://localhost:5261" })),
  ]
};
