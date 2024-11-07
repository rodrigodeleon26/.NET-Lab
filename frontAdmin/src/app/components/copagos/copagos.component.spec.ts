import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CopagosComponent } from './copagos.component';

describe('CopagosComponent', () => {
  let component: CopagosComponent;
  let fixture: ComponentFixture<CopagosComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CopagosComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CopagosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
