using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    // 실제로 씬상에서 아이템 오브젝트에 부착되는 스크립트.

    [SerializeField]
    string itemName;
    [SerializeField]
    Item item;
    SpriteRenderer renderer_;

    void Start()
    {
        renderer_ = transform.GetChild(0).GetComponent<SpriteRenderer>();
        Setting(FileManager.DataLoad<Item>(string.Format("ItemDatabase/{0}", itemName)));
    }

    public void Setting(Item item_) // 초기화
    {
        this.item = item_;
        renderer_.sprite = ResourceManager.Instance.GetItemIcon(itemName);
    }

    public Item GetItem()
    {
        StartCoroutine(Destroy());
        return this.item;
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}
