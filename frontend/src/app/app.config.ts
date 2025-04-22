import { ApplicationConfig, importProvidersFrom, provideZoneChangeDetection } from '@angular/core';
import { provideRouter, withComponentInputBinding } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';

import { routes } from './app.routes';
import { ApiModule } from './api/api.module';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes, withComponentInputBinding()),
    provideHttpClient(),

    // this should be easier, but until the NgModule related stuff is updated, we use the solution provided in https://github.com/cyclosproject/ng-openapi-gen/issues/336#issue-2704985424
    // we need to set the url manually since it is defaulted with a trailing /, causing requests to go to localhost:5261//auth/... which will always 404
    importProvidersFrom(ApiModule.forRoot({ rootUrl: "" })),
  ]
};
