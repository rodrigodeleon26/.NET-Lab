import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListMedicosComponent } from './list-medicos.component';

describe('ListMedicosComponent', () => {
  let component: ListMedicosComponent;
  let fixture: ComponentFixture<ListMedicosComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ListMedicosComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ListMedicosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
