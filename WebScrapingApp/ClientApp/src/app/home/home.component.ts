import { Component } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  cnpj = '';
  primeiroPasso = true;
  segundoPasso = false;
  loading = false;
  imageBase64 = '';
  captura = '';
  capcha = '';

  private hubConnection: HubConnection;

  ngOnInit() {
    let builder = new HubConnectionBuilder();
    this.hubConnection = builder.withUrl("http://localhost:5011/hub").build();
    this.hubConnection.start();
    this.hubConnection.on("EnviarImagemCapcha", (base64: string) => {
      this.imageBase64 = base64;
      this.loading = false;
    });
    this.hubConnection.on("EnviarCaptura", (base64: string) => {
      this.captura = base64;
      this.loading = false;
    });
  }

  pesquisar() {    
    this.hubConnection.invoke("Carregar", this.cnpj);
    this.loading = true;
    this.primeiroPasso = false;
    this.segundoPasso = true;
  }

  consultar() {
    this.hubConnection.invoke("Consultar", this.capcha);
    this.loading = true;
    this.segundoPasso = false;
  }
  limpar() {
    this.cnpj = '';
    this.primeiroPasso = true;
    this.segundoPasso = false;
    this.loading = false;
    this.imageBase64 = '';
    this.captura = '';
    this.capcha = '';
  }
}
