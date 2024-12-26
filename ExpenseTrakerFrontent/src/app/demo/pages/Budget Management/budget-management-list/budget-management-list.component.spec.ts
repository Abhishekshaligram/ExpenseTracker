import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BudgetManagementListComponent } from './budget-management-list.component';

describe('BudgetManagementListComponent', () => {
  let component: BudgetManagementListComponent;
  let fixture: ComponentFixture<BudgetManagementListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BudgetManagementListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BudgetManagementListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
