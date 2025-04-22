import { Component, inject, Input } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { Router } from '@angular/router';
import { FormControl, FormGroup, ReactiveFormsModule } from '@angular/forms';

import { BoatCreateRequest } from '../api/models';
import { BoatsService } from '../api/services';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
@Component({
  selector: 'app-boat-detail',
  imports: [ReactiveFormsModule, MatFormFieldModule, MatInputModule, MatButtonModule, MatCardModule],
  templateUrl: './boat-detail.component.html',
  styleUrl: './boat-detail.component.scss'
})
export class BoatDetailComponent {
  private _boatService = inject(BoatsService);
  private _router = inject(Router);

  private _boatId: string | null = null;
  private _readOnly = false;

  boatForm: FormGroup;

  constructor() {
    this.boatForm = new FormGroup({
      name: new FormControl("New unnamed boat"),
      description: new FormControl(''),
    });
  }

  get readOnly(): boolean {
    return this._readOnly;
  }

  @Input()
  set mode(mode: string) {
    this._readOnly = mode !== "edit";
    if (this._readOnly) {
      this.boatForm.disable();
    } else {
      this.boatForm.enable();
    }
  }

  get boatId(): string | null {
    return this._boatId;
  }

  @Input()
  set boatId(boatId: string | null) {
    if (boatId === null || boatId === "null")
      return;

    firstValueFrom(this._boatService.getBoat({ id: boatId! })).then(b => {
      this._boatId = boatId!;
      this.boatForm.setValue(b);
    });
  }

  async save() {
    const boatData = this.boatForm.getRawValue();

    if (!boatData.description) {
      boatData.description = null;
    }

    if (this._boatId === null) {
      const boatCreateRequest: BoatCreateRequest = {
        id: crypto.randomUUID(),
        data: boatData
      };

      await firstValueFrom(this._boatService.createBoat({ body: boatCreateRequest }));
    } else {
      await firstValueFrom(this._boatService.updateBoat({
        id: this._boatId,
        body: boatData
      }));
    }

    this._router.navigateByUrl("/boat-list");
  }
}
