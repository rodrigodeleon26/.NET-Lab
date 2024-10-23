import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminOrMedicoComponent } from './admin-or-medico.component';

describe('AdminOrMedicoComponent', () => {
  let component: AdminOrMedicoComponent;
  let fixture: ComponentFixture<AdminOrMedicoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AdminOrMedicoComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminOrMedicoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
