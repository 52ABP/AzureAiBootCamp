import { AppConsts } from '@shared/AppConsts';
import { UtilsService, TokenService } from '@yoyo/abp';
import { HttpHeaders } from '@angular/common/http';

export class TokenHelper {
    private static _utilsService: UtilsService = new UtilsService();
    private static _tokenService: TokenService = new TokenService();

    static createRequestHeaders(): HttpHeaders {

        let modifiedHeaders = new HttpHeaders();
        modifiedHeaders = modifiedHeaders.set("Pragma", "no-cache")
            .set("Cache-Control", "no-cache")
            .set("Expires", "Sat, 01 Jan 2000 00:00:00 GMT");

        modifiedHeaders = this.addXRequestedWithHeader(modifiedHeaders);
        modifiedHeaders = this.addAuthorizationHeaders(modifiedHeaders);
        modifiedHeaders = this.addAspNetCoreCultureHeader(modifiedHeaders);
        modifiedHeaders = this.addAcceptLanguageHeader(modifiedHeaders);
        modifiedHeaders = this.addTenantIdHeader(modifiedHeaders);

        return modifiedHeaders;
    }

    private static addXRequestedWithHeader(headers: HttpHeaders): HttpHeaders {
        if (headers) {
            headers = headers.set('X-Requested-With', 'XMLHttpRequest');
        }

        return headers;
    }

    private static addAspNetCoreCultureHeader(headers: HttpHeaders): HttpHeaders {
        let cookieLangValue = this._utilsService.getCookieValue("Abp.Localization.CultureName");
        if (cookieLangValue && headers && !headers.has('.AspNetCore.Culture')) {
            headers = headers.set('.AspNetCore.Culture', cookieLangValue);
        }

        return headers;
    }

    private static addAcceptLanguageHeader(headers: HttpHeaders): HttpHeaders {
        let cookieLangValue = this._utilsService.getCookieValue("Abp.Localization.CultureName");
        if (cookieLangValue && headers && !headers.has('Accept-Language')) {
            headers = headers.set('Accept-Language', cookieLangValue);
        }

        return headers;
    }

    private static addTenantIdHeader(headers: HttpHeaders): HttpHeaders {
        let cookieTenantIdValue = this._utilsService.getCookieValue('Abp.TenantId');
        if (cookieTenantIdValue && headers && !headers.has('Abp.TenantId')) {
            headers = headers.set('Abp.TenantId', cookieTenantIdValue);
        }

        return headers;
    }

    private static addAuthorizationHeaders(headers: HttpHeaders): HttpHeaders {
        let authorizationHeaders = headers ? headers.getAll('Authorization') : null;
        if (!authorizationHeaders) {
            authorizationHeaders = [];
        }

        if (!this.itemExists(authorizationHeaders, (item: string) => item.indexOf('Bearer ') == 0)) {
            let token = this._tokenService.getToken();
            if (token === null) {
                token = '';
            }
            if (headers && token) {
                headers = headers.set('Authorization', 'Bearer ' + token);
            }
        }

        return headers;
    }

    private static itemExists<T>(items: T[], predicate: (item: T) => boolean): boolean {
        for (let i = 0; i < items.length; i++) {
            if (predicate(items[i])) {
                return true;
            }
        }

        return false;
    }
}