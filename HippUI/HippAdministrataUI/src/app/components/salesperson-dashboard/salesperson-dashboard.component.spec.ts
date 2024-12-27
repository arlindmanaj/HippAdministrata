import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SalespersonDashboardComponent } from './salesperson-dashboard.component';

describe('SalespersonDashboardComponent', () => {
  let component: SalespersonDashboardComponent;
  let fixture: ComponentFixture<SalespersonDashboardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SalespersonDashboardComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SalespersonDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
