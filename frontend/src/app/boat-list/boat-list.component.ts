import { Component, inject, OnInit } from '@angular/core';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatMenuModule } from '@angular/material/menu';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { firstValueFrom } from 'rxjs';
import { BoatsService } from '../api/services';
import { BoatDetail } from '../api/models';
import { Router } from '@angular/router';

@Component({
  selector: 'app-boat-list',
  templateUrl: './boat-list.component.html',
  styleUrl: './boat-list.component.scss',
  imports: [
    MatGridListModule,
    MatMenuModule,
    MatIconModule,
    MatButtonModule,
    MatCardModule
  ]
})
export class BoatListComponent implements OnInit {
  private _boatsService = inject(BoatsService);
  private _router = inject(Router);

  boats: Array<BoatDetail> | null = null;

  async ngOnInit(): Promise<void> {
    await this.loadBoats();
  }

  viewBoat(boatId: string) {
    this._router.navigateByUrl("/boat-detail/" + boatId + "/view");
  }

  addBoat() {
    this._router.navigateByUrl("/boat-detail/" + null + "/edit");
  }

  editBoat(boatId: string): void {
    this._router.navigateByUrl("/boat-detail/" + boatId + "/edit");
  }

  async deleteBoat(id: string) {
    await firstValueFrom(this._boatsService.deleteBoat({ id }));
    await this.loadBoats();
  }

  private async loadBoats() {
    this.boats = await firstValueFrom(this._boatsService.listBoats());
    this.boats.sort((a, b) => a.data.name.toLowerCase() > b.data.name.toLowerCase() ? 1 : -1);
  }
}
