// import { Component,ViewEncapsulation} from '@angular/core';
// import { CommonModule } from '@angular/common';
// @Component({
//   selector: 'app-settings-modal',
//   templateUrl: './settings-modal.component.html',
//   styleUrls: ['./settings-modal.component.css'],
//   imports: [CommonModule],
//   encapsulation: ViewEncapsulation.None

// })
// export class SettingsModalComponent {
//   isOpen = false;
//   muteNotifications = localStorage.getItem('muteNotifications') === 'true';
//   toggleModal() {
//     this.isOpen = !this.isOpen; // Toggles modal state
//   }
//   toggleMuteNotifications() {
//     this.muteNotifications = !this.muteNotifications;
//     localStorage.setItem('muteNotifications', String(this.muteNotifications));
//   }
// }
// import { Component,ViewEncapsulation } from '@angular/core';
// import { CommonModule } from '@angular/common';

// @Component({
//   selector: 'app-settings-modal',
//   templateUrl: './settings-modal.component.html',
//   styleUrls: ['./settings-modal.component.css'],
//   imports: [CommonModule],
//   encapsulation: ViewEncapsulation.None

// })
// export class SettingsModalComponent {
//   isSettingsModalOpen = false;
//   muteNotifications = localStorage.getItem('muteNotifications') === 'true';

//   toggleModal() {
//         this.isSettingsModalOpen = !this.isSettingsModalOpen; // Toggles modal state
//       }
//   // Toggle mute notifications
//   toggleMuteNotifications() {
//     this.muteNotifications = !this.muteNotifications;
//     localStorage.setItem('muteNotifications', String(this.muteNotifications));
//   }
// }

import { Component, Input, ViewEncapsulation } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-settings-modal',
  templateUrl: './settings-modal.component.html',
  styleUrls: ['./settings-modal.component.css'],
  imports: [CommonModule],
  encapsulation: ViewEncapsulation.None
})
export class SettingsModalComponent {
  isSettingsModalOpen = false;
  muteNotifications = localStorage.getItem('muteNotifications') === 'true';

  @Input() isSidebarCollapsed: boolean = false; // Receives collapse state

  toggleModal() {
    this.isSettingsModalOpen = !this.isSettingsModalOpen;
  }

  toggleMuteNotifications() {
    this.muteNotifications = !this.muteNotifications;
    localStorage.setItem('muteNotifications', String(this.muteNotifications));
  }
}
