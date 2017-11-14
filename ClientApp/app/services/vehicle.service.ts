
import { Injectable } from '@angular/core';
import { Http } from '@angular/http'; 
import 'rxjs/add/operator/map';
import { SaveVehicle } from "../models/vehicle";
import { AuthHttp } from "angular2-jwt/angular2-jwt";


@Injectable()
export class VehicleService {

  private readonly vehiclesEndpoint = '/api/vehicles';

  constructor(private http: Http, private authHttp: AuthHttp) { } //so pra mandar o token junto com a requisição

  getFeatures() {
    return this.http.get('/api/features')
      .map(res => res.json());
  }

  getMakes() {
    return this.http.get('/api/makes')
      .map(res => res.json());
  }

  create(vehicle) {
    return this.authHttp.post('/api/vehicles', vehicle) //apenas troco o http, por authHttp pra mandar o token junto
      .map(res => res.json());
  }

  getVehicle(id) {
    return this.http.get('/api/vehicles/' + id)
      .map(res => res.json());
  }

  update(vehicle: SaveVehicle) {
    return this.authHttp.put('/api/vehicles/' + vehicle.id, vehicle)
      .map(res => res.json());
  }

  delete(id) {
    return this.authHttp.delete('/api/vehicles/' + id)
      .map(res => res.json());
  }

  getVehicles(filter) {
    return this.http.get('/api/vehicles' + '?' + this.toQueryString(filter))
      .map(res => res.json());
  }

  toQueryString(obj){
    var parts= [];
    for(var property in obj){

      var value = obj[property];
      if(value != null && value != undefined){
        parts.push(encodeURIComponent(property) + '=' + encodeURIComponent(value)); //para fazer o filtro   //prop=value&
      }
    }

    return parts.join('&');
  }
}
