import { CanActivate } from '@angular/router';
import { Auth } from './auth.service';
import { Injectable } from '@angular/core';

@Injectable()
export class AuthGuard implements CanActivate { //canActivate é o bloqueia as rotas

  constructor(protected auth: Auth) { }

  canActivate() {                   //isso para verificar se esta logado ou nao
    if (this.auth.authenticated())
      return true;

    window.location.href = 'https://vegaproject7.auth0.com/login?client=ef34xouCF9Awhz3NF9I3TxzXPVziniWa'; //se nao estiver logado, vai direcionar pra pagina de login
    return false;
  }
}
