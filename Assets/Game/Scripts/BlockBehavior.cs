using TMPro;
using UnityEngine;

public class BlockBehavior : MonoBehaviour
{
    Piece Piece;
    public bool hasCollided = false;
    public AudioClip DropAudio;

    public void Initialize(Piece piece)
    {
        this.hasCollided = false;
        Piece = piece;
        this.enabled = true;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("SetBlock"))
        {
            this.gameObject.tag = "SetBlock";
            hasCollided = true;
            Piece.CollisionData = col;
            this.enabled = false;
        }

        if (col.gameObject.CompareTag("FallenBoard"))
        {
            Piece.Game.Points -= 4;
            Piece.Game.TextPoints.GetComponent<TextMeshProUGUI>().text = Piece.Game.Points.ToString();

            Destroy(this.gameObject);
            AudioManager.Instance.PlaySound(DropAudio, transform.position);
            Piece.Game.SpawnNow = true;
        }
    }
}
