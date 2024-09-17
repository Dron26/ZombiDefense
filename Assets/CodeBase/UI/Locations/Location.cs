
         namespace UI.Levels
        {
            public class Location
            {
                public int Id;
                public bool IsTutorial;
                public bool IsLocked;
                public bool IsCompleted;


                public Location(int id, bool isTutorial, bool isLocked, bool isCompleted)
                {
                    Id = id;
                    IsTutorial = isTutorial;
                    IsLocked = isLocked;
                    IsCompleted = isCompleted;
                }
                
                public void SetLock(bool isLocked)
                {
                    IsLocked = isLocked;
                }

                public void SetCompleted(bool isCompleted)
                {
                    IsCompleted = isCompleted;
                }

                
            }
        }