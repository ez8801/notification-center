# notification-center

Design patterns for broadcasting information and for subscribing to broadcasts.

## Installation

### Install via git URL

To install this package, you need to edit your Unity project's `Packages/manifest.json` and add this repository as a dependency. 
``` json
{
  "dependencies": {
    "com.ez.notification-center": "https://github.com/ez8801/notification-center.git",
  }
}
```

Usage
---

To create an observer

```csharp

// Define the observer, which is a type that implements the IObserver interface.
public class ExmapleObserver : MonoBehaviour, IObserver
{
  private void Start()
  {
    // Adds an observer to the notification center to receive notification.
    NotificationCenter.Instance.AddObserver(this, R.Id.OnPaused);
  }
  
  public void HandleNotification(Notification notification)
  {
      switch (notification.id)
      {
          case R.Id.OnPaused:
              // OnPaused();
              break;
      }
  }
  
  private void OnDestroy()
  {
    NotificationCenter.Instance.RemoveObserver(this, R.Id.OnPaused);
  }
}

```

Posting Notifications

```csharp

void Pause()
{
  // Posts a notification to the notification center.
  NotificationCenter.Post(R.Id.OnPaused);
  
  // You can also posts a notification with parameters
  NotificationCenter.Post(R.Id.OnPaused, false);
  
  Notification notification = new Notification(R.Id.OnPaused, false);
  NotificationCenter.Post(notification);
}

```

License
---
Distributed under the MIT License. See LICENSE for more information.
