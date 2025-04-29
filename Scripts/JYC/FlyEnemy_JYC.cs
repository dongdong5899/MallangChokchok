using UnityEngine;

public class FlyEnemy_JYC : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] GameObject attackObject;
    public float moveSpeed = 3f;

    [SerializeField] private float dropCooldown = 10f;
    private float lastAttackTime;
    private bool hasAttacked = false;
    private bool isMoving = true;

    void Start()
    {
        lastAttackTime = Time.time;
    }

    void Update()
    {
        float fixedYPosition = transform.position.y;

        Vector3 targetPosition = new Vector3(target.position.x, fixedYPosition, target.position.z);

        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);

        if (distanceToTarget <= 5.0f)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        }

        if (isMoving)
        {
            float xDifference = Mathf.Abs(target.position.x - transform.position.x);
            float yDifference = Mathf.Abs(target.position.y - transform.position.y);

            if (xDifference <= 10f && yDifference <= 10f && Time.time - lastAttackTime >= dropCooldown)
            {
                lastAttackTime = Time.time;
                DropAttackObject();
            }
        }

    }

    private void DropAttackObject()
    {
        if (attackObject != null)
        {
            Vector3 attackDir = (target.position - transform.position).normalized;
            GameObject newAttackObject = Instantiate(attackObject, transform.position, Quaternion.identity);
            /*float yOffset = 1.0f;
            newAttackObject.transform.position = new Vector3(transform.position.x, transform.position.y - yOffset, transform.position.z);*/

            Rigidbody rb = newAttackObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                float upwardForce = 20f;
                rb.AddForce(attackDir * upwardForce, ForceMode.Impulse);
            }
        }
    }


}
