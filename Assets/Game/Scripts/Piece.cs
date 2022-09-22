using UnityEngine;

public class Piece : MonoBehaviour
{
    public Vector3 Position { get; set; }
    public GameObject Block;
    public GameObject Cube;
    public Collision Collision;
    public BlockBehavior SuperPower;
    public GameManagerScript Game;
    public FixedJoint Joint;
    public Material SuperPowerMaterial;
    public GameObject LastActivePiece { get; set; }
    public Collision CollisionData;

    public void Initialize(Vector3 position, GameObject block, GameManagerScript game)
    {
        Block = block;
        Position = position;
        Block.transform.position = Position;
        SuperPower = Block.GetComponent<BlockBehavior>();
        SuperPower.Initialize(this);
        Game = game;
    }

    public void Move(Vector3 translate)
    {  
        Block.transform.Translate(translate);
    }

    public void Rotate(Vector3 rotate)
    {
        Block.transform.Rotate(rotate);
    }

    public void ConnectBlocks(Collision col)
    {
        Joint = LastActivePiece.AddComponent<FixedJoint>();
        // sets Joint position to point of contact
        Joint.anchor = col.contacts[0].point;
        // conects the Joint to the other object
        Joint.connectedBody = col.contacts[0].otherCollider.transform.GetComponentInParent<Rigidbody>();
        // Stops objects from continuing to collide and creating more joints
        Joint.enableCollision = false;
        //LastActivePiece.GetComponent<Renderer>().material = SuperPowerMaterial;
        var lista = LastActivePiece.transform.GetChild(LastActivePiece.transform.childCount);
        Debug.Log("HMM? : " + lista);
    }
}
