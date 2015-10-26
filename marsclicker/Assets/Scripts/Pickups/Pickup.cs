using UnityEngine;
using System.Collections;

public abstract class Pickup
{

    public GameObject PickupObject { get; set; }

    public bool IsActive { get; private set; }

    public float OpacityTimer { get; private set; }
    public float OpacityChangeInterval { get; private set; }
    public float OpacityLastChangeTime { get; private set; }

    public float CurrentAlphaValue { get; private set; }
    public float AlphaChangeValue { get; private set; }

    public AudioSource PickupSound { get; private set; }

    public Pickup(float spawnX, float spawnY, Texture2D texture, AudioSource sound)
    {
        PickupSound = sound;
        AlphaChangeValue = 0.025f;
        CurrentAlphaValue = 0.0f;
        OpacityChangeInterval = 0.125f;
        IsActive = true;

        Sprite PickupSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
        GameObject obj = new GameObject();
        obj.AddComponent<SpriteRenderer>();
        obj.AddComponent<BoxCollider>();
        SpriteRenderer renderer = obj.GetComponent<SpriteRenderer>();
        renderer.sprite = PickupSprite;
        renderer.material.color = new Color(1.0f, 1.0f, 1.0f, CurrentAlphaValue);
        Vector2 rendererBounds = renderer.sprite.bounds.size;
        BoxCollider collider = obj.GetComponent<BoxCollider>();
        collider.size = rendererBounds;
        collider.center = new Vector2(rendererBounds.x / 2, rendererBounds.y / 2);

        Vector3 worldVector = Camera.main.ScreenToWorldPoint(new Vector3(spawnX, spawnY));
        renderer.transform.position = new Vector3(worldVector.x, worldVector.y, -3.0f);
        obj.name = "Pickup";
        //8 = pickups
        obj.layer = 8;
        PickupObject = obj;
        PickupObject.transform.localScale = new Vector3(0.75f, 0.75f);



    }

    public void UpdateOpacity()
    {
        OpacityTimer += Time.deltaTime;
        if (OpacityTimer > OpacityLastChangeTime + OpacityChangeInterval)
        {
            OpacityLastChangeTime = OpacityTimer;

            if (CurrentAlphaValue >= 1.0f)
            {
                AlphaChangeValue *= -1;
            }

            CurrentAlphaValue += AlphaChangeValue;
            PickupObject.GetComponent<SpriteRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, CurrentAlphaValue);

            if (CurrentAlphaValue <= 0.0f)
            {
                IsActive = false;
            }
        }
    }

    public bool IsCollected()
    {
        bool collected = false;
        LayerMask layerMask = 1 << LayerMask.NameToLayer("Pickups");
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit = new RaycastHit();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 8.0f, layerMask))
            {
                if (hit.collider.gameObject == PickupObject)
                {
                    collected = true;
                }
            }
        }
        return collected;
    }

    public abstract void OnCollect();


}
