using UnityEngine;

public class AIBehaviour : MonoBehaviour
{

    private PlatformerCharacter2D character;
    const int right = 1;
    const int left = -1;
    private int moveDirection = right;

    // Use this for initialization
    void Start()
    {
        character = FindObjectOfType<PlatformerCharacter2D>();
    }

    // Update is called once per frame
    void Update()
    {

        // Check if has platform
        if (character.HasPlatform())
        {
            if (character.IsWalled() && !character.IsBigWalled())
            {
                // Character jump
                character.Move(moveDirection, false, true);
                Debug.Log("Jump Wall");
            }
            else
            {
                // Change direction
                if (character.IsBigWalled() && !character.CanCrouch())
                {
                    if (moveDirection == right)
                    {
                        moveDirection = left;
                    }
                    else
                    {
                        moveDirection = right;
                    }
                    Debug.Log("Change Direction");
                }

                if (character.IsBigWalled() && character.CanCrouch())
                {
                    // Character walk crouched
                    character.Move(moveDirection, true, false);
                    Debug.Log("Walk crouched");
                }
                else
                {
                    // Character walk
                    character.Move(moveDirection, false, false);
                    Debug.Log("Walking");
                }
            }
        }
        else if (!character.HasPlatformBelow())
        {
            // Character jump
            character.Move(moveDirection, false, true);
            Debug.Log("Jump Spikes/Platforms");
        }
        else
        {
            // Character walk
            character.Move(moveDirection, false, false);
            Debug.Log("Walking");
        }
    }
}
