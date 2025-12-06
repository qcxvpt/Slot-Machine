using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotHandler : MonoBehaviour
{
    // Public variables
    public GameObject IconPrefab;
    public List<float> Ypositions;
    public List<float> Xpositions;
    public Sprite[] Icons;
    public float UpperBoundary;
    public float LowerBoundary;
    public float ScrollSpeed = 2f; // Speed at which the icons scroll
    public Combo comboHandler;
    private bool[] columnStopped;
    public List<List<GameObject>> ActiveIcons;
    private bool isSpinning = false;
    public Button SpinButton;
    public Slider betSlider;
    public AudioSource SpinLoop;
    public AudioSource Effects;

    // Start is called before the first frame update
void Start()
{
    // Initialize the ActiveIcons list
    ActiveIcons = new List<List<GameObject>>();
    columnStopped = new bool[Xpositions.Count]; // Initialize the stopped state array with the size of Xpositions.Count

    // Instantiate prefabs and form the grid
    InstantiateGrid();
}
    // Method to instantiate the grid
    void InstantiateGrid()
    {
        // Iterate through Xpositions to create columns
        for (int j = 0; j < Xpositions.Count; j++)
        {
            List<GameObject> column = new List<GameObject>();

            // Iterate through Ypositions to create rows within each column
            for (int i = 0; i < Ypositions.Count; i++)
            {
                // Calculate the position for the current icon
                Vector3 position = new Vector3(Xpositions[j], Ypositions[i], 0);

                // Instantiate the IconPrefab at the calculated position
                GameObject iconInstance = Instantiate(IconPrefab, position, Quaternion.identity);

                // Set the parent to keep the hierarchy clean (optional)
                iconInstance.transform.SetParent(transform);

                // Assign a random icon from the Icons array
                Sprite randomIcon = Icons[Random.Range(0, Icons.Length)];
                iconInstance.GetComponent<SpriteRenderer>().sprite = randomIcon;

                // Add the instantiated icon to the column list
                column.Add(iconInstance);
            }

            // Add the column to the ActiveIcons list
            ActiveIcons.Add(column);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If spinning, move icons down
        if (isSpinning)
        {
            SpinIcons();
        }
    }
public void StartSpin()
{
    SpinLoop.Play();
    SpinButton.interactable = false;
    betSlider.interactable = false;
    for (int i = 0; i < columnStopped.Length; i++)
    {
        columnStopped[i] = false; // Reset the stopped state for each column
    }

    int startAlgorithm = Random.Range(0, 3); // Choose a starting algorithm randomly

    switch (startAlgorithm)
    {
        case 0:
            Debug.Log("All at once");
            for (int i = 0; i < Xpositions.Count; i++)
            {
                StartCoroutine(SpinSingleRow(i));
            }
            break;
        case 1:
            Debug.Log("Perfect order");
            StartCoroutine(StartRowsOneByOne(0.3f));
            break;
        case 2:
            Debug.Log("Random order");
            StartCoroutine(StartRowsOneByOneRandomOrder(0.3f));
            break;
    }
}
private IEnumerator StartRowsOneByOne(float delay)
{
    for (int i = 0; i < Xpositions.Count; i++)
    {
        StartCoroutine(SpinSingleRow(i));
        yield return new WaitForSeconds(delay);
    }
}
public void RandomTest()
{
    StartCoroutine(StartRowsOneByOneRandomOrder(0.3f));
}

private IEnumerator StartRowsOneByOneRandomOrder(float delay)
{
    List<int> indices = new List<int>();
    for (int i = 0; i < Xpositions.Count; i++) indices.Add(i);

    while (indices.Count > 0)
    {
        int randomIndex = Random.Range(0, indices.Count);
        StartCoroutine(SpinSingleRow(indices[randomIndex]));
        indices.RemoveAt(randomIndex);
        yield return new WaitForSeconds(delay);
    }
}


private IEnumerator SpinSingleRow(int columnIndex)
{
    float spinDuration = 3f; // Duration for spinning
    float startTime = Time.time;

    while (Time.time - startTime < spinDuration)
    {
        // Spin only the specified column
        foreach (var icon in ActiveIcons[columnIndex])
        {
            // Move the icon down
            icon.transform.position -= new Vector3(0, ScrollSpeed * Time.deltaTime, 0);

            // If the icon reaches the lower boundary, wrap it to the upper boundary and change the sprite
            if (icon.transform.position.y < LowerBoundary)
            {
                icon.transform.position = new Vector3(icon.transform.position.x, UpperBoundary, icon.transform.position.z);
                Sprite randomIcon = GetRandomIcon();
                icon.GetComponent<SpriteRenderer>().sprite = randomIcon;
            }
        }

        yield return null;
    }

    StopColumn(columnIndex);
}

private Sprite GetRandomIcon()
{
    float randomValue = Random.Range(0f,1f);

    if (randomValue < 0.05f)
    {
        // 15% chance to turn into icon with index 8 (Wild)
        return Icons[8];
    }
    else if (randomValue < 0.1f)
    {
        // 5% chance to turn into icon with index 9 (Scatter)
        return Icons[9];
    }
    else
    {
        // 80% chance to turn into any other icon
        int randomIndex = Random.Range(0, 8); // Icons from 0 to 7
        return Icons[randomIndex];
    }
}

private void StopColumn(int columnIndex)
{
    // Create a HashSet to track which Y positions have already been used
    HashSet<float> usedYPositions = new HashSet<float>();

    // Snap each icon in the column to the closest available Y position
    foreach (var icon in ActiveIcons[columnIndex])
    {
        float closestLowerY = FindClosestAvailableYPosition(icon.transform.position.y, usedYPositions); // Pass the usedYPositions set
        icon.transform.position = new Vector3(icon.transform.position.x, closestLowerY, icon.transform.position.z);

        // Mark this Y position as used
        usedYPositions.Add(closestLowerY);
    }

    columnStopped[columnIndex] = true; // Mark this column as stopped

    // Check if all columns have stopped
    if (AreAllColumnsStopped())
    {
        SpinLoop.Stop();
        comboHandler.CheckAllCombos();
        comboHandler.CountIconsWithIndex8();
        comboHandler.CountIconsWithIndex9();
        SpinButton.interactable = true;
        betSlider.interactable = true;
    }
}
public bool AreAllColumnsStopped()
{
    for (int i = 0; i < columnStopped.Length; i++)
    {
        if (!columnStopped[i]) return false; // Return false if any column hasn't stopped yet
    }
    return true; // Return true only if all columns have stopped
}
    // Method to stop spinning
    public void StopSpin()
    {
isSpinning = false;
    SnapIconsToGrid();
    }

    // Method to spin icons
void SpinIcons()
{
    if (!isSpinning) return; // Exit if not spinning

    foreach (var column in ActiveIcons)
    {
        foreach (var icon in column)
        {
            // Move the icon down
            icon.transform.position -= new Vector3(0, ScrollSpeed * Time.deltaTime, 0);

            // If the icon reaches the lower boundary, wrap it to the upper boundary and change the sprite
            if (icon.transform.position.y < LowerBoundary)
            {
                icon.transform.position = new Vector3(icon.transform.position.x, UpperBoundary, icon.transform.position.z);
                Sprite randomIcon = Icons[Random.Range(0, Icons.Length)];
                icon.GetComponent<SpriteRenderer>().sprite = randomIcon;
            }
        }
    }
}


// Method to snap icons to the closest lower Y position
void SnapIconsToGrid()
{
    // Create a list to track which Y positions have been occupied
    HashSet<float> usedYPositions = new HashSet<float>();

    foreach (var column in ActiveIcons)
    {
        foreach (var icon in column)
        {
            // Find the closest Y position that hasn't been used yet
            float closestLowerY = FindClosestAvailableYPosition(icon.transform.position.y, usedYPositions);

            // Snap the icon to the closest available Y position
            icon.transform.position = new Vector3(icon.transform.position.x, closestLowerY, icon.transform.position.z);

            // Mark this Y position as used
            usedYPositions.Add(closestLowerY);
        }
    }
}
float FindClosestAvailableYPosition(float currentY, HashSet<float> usedYPositions)
{
    float closestY = LowerBoundary; // Start with the lower boundary as the closest lower position
    float smallestDistance = Mathf.Infinity;

    foreach (float yPos in Ypositions)
    {
        if (!usedYPositions.Contains(yPos)) // Only consider unused Y positions
        {
            float distance = Mathf.Abs(yPos - currentY);
            if (distance < smallestDistance)
            {
                closestY = yPos;
                smallestDistance = distance;
            }
        }
    }

    return closestY;
}
}