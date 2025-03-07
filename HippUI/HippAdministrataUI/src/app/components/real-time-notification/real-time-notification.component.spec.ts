import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RealTimeNotificationComponent } from './real-time-notification.component';

describe('RealTimeNotificationComponent', () => {
  let component: RealTimeNotificationComponent;
  let fixture: ComponentFixture<RealTimeNotificationComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RealTimeNotificationComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RealTimeNotificationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
