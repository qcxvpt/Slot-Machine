using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Combo : MonoBehaviour
{
    // List of combinations to check
    public List<Combination> combinations = new List<Combination>();

    // A reference to the SlotHandler for accessing icons
    public SlotHandler slotHandler;

    // A reference to positions of each row and column (X, Y)
    public Vector2[,] positionMatrix = new Vector2[5, 5];
    public GameObject shinePrefab;
    public GameObject wildShine;
    public GameObject scatterShine;
    public float balance;
    public Text balanceText;
    public Text betText;
    public float bet;
    public Slider chooseBet;
    public int freespins = 0;
    public Text ButtonState;
    public Text freespinsCounter;
    public float winnings = 0f;
    public AudioSource Effects;
    public AudioClip[] clips;
    public float scatter_bet = 0f;
    public Text winningsDisplay;
    public float wildWinnings = 0f;

    private Dictionary<int, float> iconValues = new Dictionary<int, float>
    {
        { 0, 1f }, { 1, 1.25f }, { 2, 1.5f }, { 3, 1.75f },
        { 4, 2f }, { 5, 2.5f }, { 6, 3f }, { 7, 4f }
    };

    void Start()
    {
        // Initialize the position matrix
        InitializePositionMatrix();

        // Add some example combinations to the list (you can customize these)
        InitializeCombinations();
    }
    void Update()
    {
        if(balance < 0.25f)
        {
            balance = 1000f;
        }
        if(freespins > 0)
        {
            chooseBet.value = scatter_bet;
            chooseBet.gameObject.SetActive(false);
            ButtonState.text = "Free Spin";
        }
        else
        {
            chooseBet.gameObject.SetActive(true);
            ButtonState.text = "Spin";
        }
        freespinsCounter.text = "Free spins: " + freespins.ToString();
        balanceText.text = "$" + balance.ToString("F2");
        betText.text = "Bet: " + bet.ToString("F2");
        bet = chooseBet.value;
        winningsDisplay.text = (winnings+wildWinnings).ToString("F2") + "$";
    }
public void Spinner()
{
    if(freespins > 0)
    {
        wildWinnings = 0;
        winnings = 0;
        freespins--;
        slotHandler.StartSpin();
    }
    else
    {
        if(balance >= bet)
        {
        winnings = 0;
        wildWinnings = 0;
        balance -= bet;
        slotHandler.StartSpin();
        }
        else
        {
        Effects.PlayOneShot(clips[0]);
        }
    }
    foreach (GameObject ring in GameObject.FindGameObjectsWithTag("Shine"))
    {
        if (ring.scene.isLoaded)  // Ensure it's an instantiated object in the scene, not the prefab itself
        {
            Destroy(ring);
        }
    }
}


    // Initialize the position matrix for combo checking (map X and Y positions)
    void InitializePositionMatrix()
    {
        for (int row = 0; row < 5; row++)
        {
            for (int col = 0; col < 5; col++)
            {
                positionMatrix[row, col] = new Vector2(slotHandler.Xpositions[col], slotHandler.Ypositions[row]);
            }
        }
    }
public void CheatRoll()
    {
            foreach (GameObject ring in GameObject.FindGameObjectsWithTag("Shine"))
    {
        if (ring.scene.isLoaded)  // Ensure it's an instantiated object in the scene, not the prefab itself
        {
            Destroy(ring);
        }
    }
        int[,]spriteMatrix = new int[,]
{
    { 0, 1, 2, 3, 4 },  // Invisible first column and others
    { 7, 5, 4, 9, 7 },  // Visible second row
    { 3, 2, 3, 1, 7 },  // Visible third row
    { 7, 1, 3, 5, 7 },  // Visible fourth row
    { 0, 1, 2, 3, 4 }   // Invisible last column and others
};
        for (int row = 0; row < 5; row++) // Loop through 5 rows
        {
            for (int col = 0; col < 5; col++) // Loop through 5 columns
            {
                // Get the position in the grid
                Vector2 positionToSet = positionMatrix[row, col];
                GameObject icon = slotHandler.GetIconInstanceAtPosition(positionToSet);

                if (icon != null)
                {
                    // Assign the sprite based on the spriteMatrix
                    int spriteIndex = spriteMatrix[row, col]; // Get the sprite index from the matrix
                    if (spriteIndex >= 0 && spriteIndex < slotHandler.Icons.Length)
                    {
                        icon.GetComponent<SpriteRenderer>().sprite = slotHandler.Icons[spriteIndex];
                    }
                }
            }
        }
        CountIconsWithIndex8();
        CountIconsWithIndex9();
        CheckAllCombos();
    }
    int GetIconIndex(Sprite iconSprite)
    {
        for (int i = 0; i < slotHandler.Icons.Length; i++)
        {
            if (slotHandler.Icons[i] == iconSprite)
            {
                return i;
            }
        }
        return -1; // Return -1 if the icon sprite is not found
    }
    public void CountIconsWithIndex8()
{
    int count = 0;
    List<GameObject> iconsToHighlight = new List<GameObject>();

    bool[,] checkMatrix = new bool[,]
    {
        { false, false, false, false, false },
        { true, true, true, true, true },
        { true, true, true, true, true },
        { true, true, true, true, true },
        { false, false, false, false, false }
    };

    for (int row = 0; row < 5; row++)
    {
        for (int col = 0; col < 5; col++)
        {
            if (checkMatrix[row, col])
            {
                Vector2 positionToCheck = positionMatrix[row, col];
                GameObject icon = slotHandler.GetIconInstanceAtPosition(positionToCheck);

                if (icon != null && GetIconIndex(icon.GetComponent<SpriteRenderer>().sprite) == 8)
                {
                    count++;
                    iconsToHighlight.Add(icon);
                }
            }
        }
    }
    if(count > 0)
    {
        Effects.PlayOneShot(clips[2]);
    }
    wildWinnings += (bet*count)/2;
    balance += wildWinnings;
    Debug.Log($"Wild winnings: {wildWinnings}");
    HighlightWildIcons(iconsToHighlight);  // Highlight the icons with the "Shine" prefab
}
public void CountIconsWithIndex9()
{
    int count = 0;
    List<GameObject> iconsToHighlight = new List<GameObject>();

    bool[,] checkMatrix = new bool[,]
    {
        { false, false, false, false, false },
        { true, true, true, true, true },
        { true, true, true, true, true },
        { true, true, true, true, true },
        { false, false, false, false, false }
    };

    for (int row = 0; row < 5; row++)
    {
        for (int col = 0; col < 5; col++)
        {
            if (checkMatrix[row, col])
            {
                Vector2 positionToCheck = positionMatrix[row, col];
                GameObject icon = slotHandler.GetIconInstanceAtPosition(positionToCheck);

                if (icon != null && GetIconIndex(icon.GetComponent<SpriteRenderer>().sprite) == 9)
                {
                    count++;
                    iconsToHighlight.Add(icon);
                }
            }
        }
    }

    if (count >= 15)
    {
        freespins = 20;
        scatter_bet = 20f;
        Debug.Log($"Player gets {freespins} free spins for finding {count} scatter icons.");
        HighlightScatterIcons(iconsToHighlight);
        Effects.PlayOneShot(clips[1]);
    }
    else if (count >= 12)
    {
        freespins = 15;
        scatter_bet = 17.5f;
        Debug.Log($"Player gets {freespins} free spins for finding {count} scatter icons.");
        HighlightScatterIcons(iconsToHighlight);
        Effects.PlayOneShot(clips[1]);
    }
    else if (count >= 9)
    {
        freespins = 10;
        scatter_bet = 15f;
        Debug.Log($"Player gets {freespins} free spins for finding {count} scatter icons.");
        HighlightScatterIcons(iconsToHighlight);
        Effects.PlayOneShot(clips[1]);
    }
    else if (count >= 6)
    {
        freespins = 10;
        scatter_bet = 12.5f;
        Debug.Log($"Player gets {freespins} free spins for finding {count} scatter icons.");
        HighlightScatterIcons(iconsToHighlight);
        Effects.PlayOneShot(clips[1]);
    }
    else if (count >= 3)
    {
        freespins = 10;
        scatter_bet = 10f;
        Debug.Log($"Player gets {freespins} free spins for finding {count} scatter icons.");
        HighlightScatterIcons(iconsToHighlight);
        Effects.PlayOneShot(clips[1]);
    }
    else
    {
        Debug.Log("No free spins awarded.");
    }
}
public void CheckAllCombos()
{
    Combination bestCombo = null;
    float highestComboValue = 0f;
    List<GameObject> bestComboIcons = new List<GameObject>();
    float totalIconValue = 0f;

    foreach (var combination in combinations)
    {
        var result = CheckCombination(combination);  // Now using ValueTuple
        if (result.Item1 > highestComboValue)
        {
            highestComboValue = result.Item1;
            bestCombo = combination;
            bestComboIcons = result.Item2;
            totalIconValue = result.Item1;  // This includes the total icon value from the combo
        }
    }

    if (bestCombo != null)
    {
        // Update player's balance using the formula
        winnings += bet * highestComboValue;
        Debug.Log($"Best Combo: {bestCombo.comboName}, Value: {winnings}");
        balance += winnings;
        Effects.PlayOneShot(clips[3]);

        HighlightComboIcons(bestComboIcons);  // Highlight the icons making the highest combo
    }
}

private void HighlightComboIcons(List<GameObject> icons)
{
    foreach (var icon in icons)
    {
        Vector3 iconPosition = icon.transform.position;
        Instantiate(shinePrefab, iconPosition, Quaternion.identity);  // Spawn Shine prefab at each icon's position
        shinePrefab.gameObject.tag = "Shine";
    }
}
private void HighlightWildIcons(List<GameObject> icons)
{
    foreach (var icon in icons)
    {
        Vector3 iconPosition = icon.transform.position;
        Instantiate(wildShine, iconPosition, Quaternion.identity);  // Spawn Shine prefab at each icon's position
        shinePrefab.gameObject.tag = "Shine";
    }
}
private void HighlightScatterIcons(List<GameObject> icons)
{
    foreach (var icon in icons)
    {
        Vector3 iconPosition = icon.transform.position;
        Instantiate(scatterShine, iconPosition, Quaternion.identity);  // Spawn Shine prefab at each icon's position
        shinePrefab.gameObject.tag = "Shine";
    }
}
(float, List<GameObject>) CheckCombination(Combination combination)
{
    bool isCombo = true;
    Sprite firstIconSprite = null;
    float totalIconValue = 0f;
    List<GameObject> comboIcons = new List<GameObject>();

    for (int row = 0; row < 5; row++)
    {
        for (int col = 0; col < 5; col++)
        {
            if (combination.boolMatrix[row, col])
            {
                Vector2 positionToCheck = positionMatrix[row, col];
                GameObject icon = slotHandler.GetIconInstanceAtPosition(positionToCheck);

                if (icon == null)
                {
                    return (0f, new List<GameObject>());  // Return empty tuple if no icon found
                }

                Sprite iconSprite = icon.GetComponent<SpriteRenderer>().sprite;
                int iconIndex = GetIconIndex(iconSprite);

                if (firstIconSprite == null)
                {
                    firstIconSprite = iconSprite;
                }
                else if (iconSprite != firstIconSprite)
                {
                    isCombo = false;
                }

                if (iconValues.ContainsKey(iconIndex))
                {
                    totalIconValue = iconValues[iconIndex];
                }

                comboIcons.Add(icon);  // Collect all icons involved in the combo
            }
        }
    }

    if (isCombo && firstIconSprite != null)
    {
        return (combination.comboValue * totalIconValue, comboIcons);  // Return combo value and icons
    }

    return (0f, new List<GameObject>());
}
// Method to initialize some example combinations (customize this as needed)
    void InitializeCombinations()
    {
        bool x = true;
        bool o = false;
        bool[,] comboMatrix1 = new bool[,]
        {
            { o, o, o, o, o },
            { o, o, o, o, x },
            { o, o, o, o, x },
            { o, o, o, o, x },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix1, 0.5f, "Right Vertical"));

        bool[,] comboMatrix2 = new bool[,]
        {
            { o, o, o, o, o },
            { o, o, o, x, o },
            { o, o, o, x, o },
            { o, o, o, x, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix2, 0.51f, "Half-right vertical"));

        bool[,] comboMatrix3 = new bool[,]
        {
            { o, o, o, o, o },
            { o, o, x, o, o },
            { o, o, x, o, o },
            { o, o, x, o, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix3, 0.55f, "Middle vertical"));

        bool[,] comboMatrix4 = new bool[,]
        {
            { o, o, o, o, o },
            { o, x, o, o, o },
            { o, x, o, o, o },
            { o, x, o, o, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix4, 0.54f, "Half-left vertical"));

        bool[,] comboMatrix5 = new bool[,]
        {
            { o, o, o, o, o },
            { x, o, o, o, o },
            { x, o, o, o, o },
            { x, o, o, o, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix5, 0.55f, "Left vertical"));

        bool[,] comboMatrix6 = new bool[,]
        {
            { o, o, o, o, o },
            { x, x, x, o, o },
            { o, o, o, o, o },
            { o, o, o, o, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix6, 0.8f, "North-Western Horizontal 3"));

        bool[,] comboMatrix7 = new bool[,]
        {
            { o, o, o, o, o },
            { o, x, x, x, o },
            { o, o, o, o, o },
            { o, o, o, o, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix7, 0.85f, "Northern Horizontal 3"));

        bool[,] comboMatrix8 = new bool[,]
        {
            { o, o, o, o, o },
            { o, o, x, x, x },
            { o, o, o, o, o },
            { o, o, o, o, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix8, 0.78f, "North-Eastern Horizontal 3"));

        bool[,] comboMatrix9 = new bool[,]
        {
            { o, o, o, o, o },
            { o, o, o, o, o },
            { x, x, x, o, o },
            { o, o, o, o, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix9, 0.82f, "Western Horizontal 3"));

        bool[,] comboMatrix10 = new bool[,]
        {
            { o, o, o, o, o },
            { o, o, o, o, o },
            { o, x, x, x, o },
            { o, o, o, o, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix10, 0.9f, "Central Horizontal 3"));

        bool[,] comboMatrix11 = new bool[,]
        {
            { o, o, o, o, o },
            { o, o, o, o, o },
            { o, o, x, x, x },
            { o, o, o, o, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix11, 0.84f, "Eastern Horizontal 3"));

                bool[,] comboMatrix12 = new bool[,]
        {
            { o, o, o, o, o },
            { o, o, o, o, o },
            { o, o, o, o, o },
            { x, x, x, o, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix12, 0.77f, "South-Western Horizontal 3"));

        bool[,] comboMatrix13 = new bool[,]
        {
            { o, o, o, o, o },
            { o, o, o, o, o },
            { o, o, o, o, o },
            { o, x, x, x, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix13, 0.79f, "Southern Horizontal 3"));

        bool[,] comboMatrix14 = new bool[,]
        {
            { o, o, o, o, o },
            { o, o, o, o, o },
            { o, o, o, o, o },
            { o, o, x, x, x },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix14, 0.75f, "South-Eastern Horizontal 3"));

        bool[,] comboMatrix15 = new bool[,]
        {
            { o, o, o, o, o },
            { x, o, o, o, o },
            { o, x, o, o, o },
            { o, o, x, o, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix15, 0.93f, "Left-Left Diagonal"));

        bool[,] comboMatrix16 = new bool[,]
        {
            { o, o, o, o, o },
            { o, x, o, o, o },
            { o, o, x, o, o },
            { o, o, o, x, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix16, 0.95f, "Left-Centre Diagonal"));

        bool[,] comboMatrix17 = new bool[,]
        {
            { o, o, o, o, o },
            { o, o, x, o, o },
            { o, o, o, x, o },
            { o, o, o, o, x },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix17, 0.91f, "Left-Right Diagonal"));

        bool[,] comboMatrix18 = new bool[,]
        {
            { o, o, o, o, o },
            { o, o, x, o, o },
            { o, x, o, o, o },
            { x, o, o, o, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix18, 0.92f, "Right-Left Diagonal"));

        bool[,] comboMatrix19 = new bool[,]
        {
            { o, o, o, o, o },
            { o, o, o, x, o },
            { o, o, x, o, o },
            { o, x, o, o, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix19, 0.94f, "Right-Centre Diagonal"));

        bool[,] comboMatrix20 = new bool[,]
        {
            { o, o, o, o, o },
            { o, o, o, o, x },
            { o, o, o, x, o },
            { o, o, x, o, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix20, 0.9f, "Right-Right Diagonal"));

        bool[,] comboMatrix21 = new bool[,]
        {
            { o, o, o, o, o },
            { o, o, x, o, o },
            { o, x, o, x, o },
            { o, o, x, o, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix21, 1.25f, "Rombus Middle"));

        bool[,] comboMatrix22 = new bool[,]
        {
            { o, o, o, o, o },
            { o, x, o, o, o },
            { x, o, x, o, o },
            { o, x, o, o, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix22, 1.15f, "Rombus Left"));

        bool[,] comboMatrix23 = new bool[,]
        {
            { o, o, o, o, o },
            { o, o, o, x, o },
            { o, o, x, o, x },
            { o, o, o, x, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix23, 1.2f, "Rombus Right"));

        bool[,] comboMatrix24 = new bool[,]
        {
            { o, o, o, o, o },
            { x, o, o, o, o },
            { x, o, o, o, o },
            { x, x, x, o, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix24, 1.4f, "L Left"));

        bool[,] comboMatrix25 = new bool[,]
        {
            { o, o, o, o, o },
            { o, x, o, o, o },
            { o, x, o, o, o },
            { x, x, x, o, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix25, 1.45f, "T Upside Left"));

        bool[,] comboMatrix26 = new bool[,]
        {
            { o, o, o, o, o },
            { o, o, x, o, o },
            { o, o, x, o, o },
            { x, x, x, o, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix26, 1.42f, "L Mirror Left"));

        bool[,] comboMatrix27 = new bool[,]
        {
            { o, o, o, o, o },
            { o, o, x, o, o },
            { o, o, x, o, o },
            { o, x, x, x, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix27, 1.6f, "T Upside Middle"));

        bool[,] comboMatrix28 = new bool[,]
        {
            { o, o, o, o, o },
            { o, x, o, o, o },
            { o, x, o, o, o },
            { o, x, x, x, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix28, 1.55f, "L Middle"));

        bool[,] comboMatrix29 = new bool[,]
        {
            { o, o, o, o, o },
            { o, o, o, x, o },
            { o, o, o, x, o },
            { o, x, x, x, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix29, 1.53f, "L Mirror Middle"));

        bool[,] comboMatrix30 = new bool[,]
        {
            { o, o, o, o, o },
            { o, o, o, x, o },
            { o, o, o, x, o },
            { o, o, x, x, x },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix30, 1.5f, "T Upside Right"));

        bool[,] comboMatrix31 = new bool[,]
        {
            { o, o, o, o, o },
            { o, o, x, o, o },
            { o, o, x, o, o },
            { o, o, x, x, x },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix31, 1.48f, "L Mirror Right"));

        bool[,] comboMatrix32 = new bool[,]
        {
            { o, o, o, o, o },
            { o, o, o, o, x },
            { o, o, o, o, x },
            { o, o, x, x, x },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix32, 1.47f, "L Right"));

        bool[,] comboMatrix33 = new bool[,]
        {
            { o, o, o, o, o },
            { x, x, x, o, o },
            { x, o, o, o, o },
            { x, o, o, o, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix33, 1.65f, "L Upside Left"));

        bool[,] comboMatrix34 = new bool[,]
        {
            { o, o, o, o, o },
            { x, x, x, o, o },
            { o, x, o, o, o },
            { o, x, o, o, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix34, 1.68f, "T Left"));

        bool[,] comboMatrix35 = new bool[,]
        {
            { o, o, o, o, o },
            { x, x, x, o, o },
            { o, o, x, o, o },
            { o, o, x, o, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix35, 1.63f, "L Upside Mirror Left"));

        bool[,] comboMatrix36 = new bool[,]
        {
            { o, o, o, o, o },
            { o, x, x, x, o },
            { o, o, x, o, o },
            { o, o, x, o, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix36, 1.8f, "T Middle"));

        bool[,] comboMatrix37 = new bool[,]
        {
            { o, o, o, o, o },
            { o, x, x, x, o },
            { o, x, o, o, o },
            { o, x, o, o, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix37, 1.75f, "L Upside Middle"));

        bool[,] comboMatrix38 = new bool[,]
        {
            { o, o, o, o, o },
            { o, x, x, x, o },
            { o, o, o, x, o },
            { o, o, o, x, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix38, 1.73f, "L Upside Mirror Middle"));

        bool[,] comboMatrix39 = new bool[,]
        {
            { o, o, o, o, o },
            { o, o, x, x, x },
            { o, o, o, x, o },
            { o, o, o, x, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix39, 1.72f, "T Right"));

        bool[,] comboMatrix40 = new bool[,]
        {
            { o, o, o, o, o },
            { o, o, x, x, x },
            { o, o, x, o, o },
            { o, o, x, o, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix40, 1.69f, "L Upside Right"));

        bool[,] comboMatrix41 = new bool[,]
        {
            { o, o, o, o, o },
            { o, o, x, x, x },
            { o, o, o, o, x },
            { o, o, o, o, x },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix41, 1.7f, "L Upside Mirror Right"));

        bool[,] comboMatrix42 = new bool[,]
        {
            { o, o, o, o, o },
            { o, x, o, o, o },
            { x, o, x, o, x },
            { o, o, o, x, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix42, 1.95f, "Spring Left"));

        bool[,] comboMatrix43 = new bool[,]
        {
            { o, o, o, o, o },
            { o, o, o, x, o },
            { x, o, x, o, x },
            { o, x, o, o, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix43, 1.94f, "Spring Right"));

        bool[,] comboMatrix44 = new bool[,]
        {
            { o, o, o, o, o },
            { x, x, x, o, o },
            { o, o, o, o, o },
            { x, x, x, o, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix44, 2.1f, "Equal Left Left"));

        bool[,] comboMatrix45 = new bool[,]
        {
            { o, o, o, o, o },
            { x, x, x, o, o },
            { o, o, o, o, o },
            { o, x, x, x, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix45, 2.12f, "Equal Left Middle"));

        bool[,] comboMatrix46 = new bool[,]
        {
            { o, o, o, o, o },
            { x, x, x, o, o },
            { o, o, o, o, o },
            { o, o, x, x, x },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix46, 2.13f, "Equal Left Right"));

        bool[,] comboMatrix47 = new bool[,]
        {
            { o, o, o, o, o },
            { o, x, x, x, o },
            { o, o, o, o, o },
            { x, x, x, o, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix47, 2.14f, "Equal Middle Left"));

        bool[,] comboMatrix48 = new bool[,]
        {
            { o, o, o, o, o },
            { o, x, x, x, o },
            { o, o, o, o, o },
            { o, x, x, x, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix48, 2.2f, "Equal Middle Middle"));
        
        bool[,] comboMatrix49 = new bool[,]
        {
            { o, o, o, o, o },
            { o, x, x, x, o },
            { o, o, o, o, o },
            { o, o, x, x, x },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix49, 2.19f, "Equal Middle Right"));
                
        bool[,] comboMatrix50 = new bool[,]
        {
            { o, o, o, o, o },
            { o, o, x, x, x },
            { o, o, o, o, o },
            { x, x, x, o, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix50, 2.15f, "Equal Right Left"));
                        
        bool[,] comboMatrix51 = new bool[,]
        {
            { o, o, o, o, o },
            { o, o, x, x, x },
            { o, o, o, o, o },
            { o, x, x, x, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix51, 2.18f, "Equal Right Middle"));
                                
        bool[,] comboMatrix52 = new bool[,]
        {
            { o, o, o, o, o },
            { o, o, x, x, x },
            { o, o, o, o, o },
            { o, o, x, x, x },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix52, 2.17f, "Equal Right Right"));
        
        bool[,] comboMatrix53 = new bool[,]
        {
            { o, o, o, o, o },
            { o, o, o, o, o },
            { o, x, x, x, o },
            { x, o, o, o, x },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix53, 2.22f, "Plate Down Down"));

        bool[,] comboMatrix54 = new bool[,]
        {
            { o, o, o, o, o },
            { o, o, o, o, o },
            { x, o, o, o, x },
            { o, x, x, x, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix54, 2.21f, "Plate Up Down"));
        
        bool[,] comboMatrix55 = new bool[,]
        {
            { o, o, o, o, o },
            { o, x, x, x, o },
            { x, o, o, o, x },
            { o, o, o, o, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix55, 2.25f, "Plate Down Up"));

        bool[,] comboMatrix56 = new bool[,]
        {
            { o, o, o, o, o },
            { x, o, o, o, x },
            { o, x, x, x, o },
            { o, o, o, o, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix56, 2.23f, "Plate Up Up"));
        
        bool[,] comboMatrix57 = new bool[,]
        {
            { o, o, o, o, o },
            { o, o, o, o, o },
            { o, x, o, x, o },
            { x, o, x, o, x },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix57, 2.4f, "M Down"));
                
        bool[,] comboMatrix58 = new bool[,]
        {
            { o, o, o, o, o },
            { o, o, o, o, o },
            { x, o, x, o, x },
            { o, x, o, x, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix58, 2.45f, "W Down"));
                        
        bool[,] comboMatrix59 = new bool[,]
        {
            { o, o, o, o, o },
            { o, x, o, x, o },
            { x, o, x, o, x },
            { o, o, o, o, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix59, 2.5f, "M Up"));
           
        bool[,] comboMatrix60 = new bool[,]
        {
            { o, o, o, o, o },
            { x, o, x, o, x },
            { o, x, o, x, o },
            { o, o, o, o, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix60, 2.55f, "W Up"));
                   
        bool[,] comboMatrix61 = new bool[,]
        {
            { o, o, o, o, o },
            { o, o, x, o, o },
            { o, x, o, x, o },
            { x, o, o, o, x },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix61, 2.75f, "V up"));
                   
        bool[,] comboMatrix62 = new bool[,]
        {
            { o, o, o, o, o },
            { x, o, o, o, x },
            { o, x, o, x, o },
            { o, o, x, o, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix62, 2.77f, "V down"));

        bool[,] comboMatrix63 = new bool[,]
        {
            { o, o, o, o, o },
            { x, x, o, o, o },
            { o, o, x, o, o },
            { o, o, o, x, x },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix63, 2.95f, "Half-schromosome left"));

        bool[,] comboMatrix64 = new bool[,]
        {
            { o, o, o, o, o },
            { o, o, o, x, x },
            { o, o, x, o, o },
            { x, x, o, o, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix64, 2.9f, "Half-schromosome right"));

        bool[,] comboMatrix65 = new bool[,]
        {
            { o, o, o, o, o },
            { o, o, o, o, o },
            { o, o, x, o, o },
            { x, x, o, x, x },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix65, 3f, "Half-schromosome down down"));

        bool[,] comboMatrix66 = new bool[,]
        {
            { o, o, o, o, o },
            { o, o, o, o, o },
            { x, x, o, x, x },
            { o, o, x, o, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix66, 3.05f, "Half-schromosome down up"));

        bool[,] comboMatrix67 = new bool[,]
        {
            { o, o, o, o, o },
            { o, o, x, o, o },
            { x, x, o, x, x },
            { o, o, o, o, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix67, 3.07f, "Half-schromosome up down"));

        bool[,] comboMatrix68 = new bool[,]
        {
            { o, o, o, o, o },
            { x, x, o, x, x },
            { o, o, x, o, o },
            { o, o, o, o, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix68, 3.15f, "Half-schromosome up up"));

        bool[,] comboMatrix69 = new bool[,]
        {
            { o, o, o, o, o },
            { o, x, o, o, o },
            { x, x, x, o, o },
            { o, x, o, o, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix69, 3.2f, "Straight cross left"));

        bool[,] comboMatrix70 = new bool[,]
        {
            { o, o, o, o, o },
            { o, o, x, o, o },
            { o, x, x, x, o },
            { o, o, x, o, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix70, 3.25f, "Straight cross middle"));

        bool[,] comboMatrix71 = new bool[,]
        {
            { o, o, o, o, o },
            { o, o, o, x, o },
            { o, o, x, x, x },
            { o, o, o, x, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix71, 3.19f, "Straight cross right"));

        bool[,] comboMatrix72 = new bool[,]
        {
            { o, o, o, o, o },
            { x, x, x, x, x },
            { o, o, o, o, o },
            { o, o, o, o, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix72, 3.4f, "Horizontal 5 Up"));

        bool[,] comboMatrix73 = new bool[,]
        {
            { o, o, o, o, o },
            { o, o, o, o, o },
            { x, x, x, x, x },
            { o, o, o, o, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix73, 3.5f, "Horizontal 5 Middle"));

        bool[,] comboMatrix74 = new bool[,]
        {
            { o, o, o, o, o },
            { o, o, o, o, o },
            { o, o, o, o, o },
            { x, x, x, x, x },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix74, 3.4f, "Horizontal 5 Down"));

        bool[,] comboMatrix75 = new bool[,]
        {
            { o, o, o, o, o },
            { x, o, o, o, o },
            { x, x, x, x, x },
            { x, o, o, o, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix75, 4f, "T left"));

        bool[,] comboMatrix76 = new bool[,]
        {
            { o, o, o, o, o },
            { o, x, o, o, o },
            { x, x, x, x, x },
            { o, x, o, o, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix76, 3.95f, "t left"));

        bool[,] comboMatrix77 = new bool[,]
        {
            { o, o, o, o, o },
            { o, o, x, o, o },
            { x, x, x, x, x },
            { o, o, x, o, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix77, 4.1f, "Long cross"));

        bool[,] comboMatrix78 = new bool[,]
        {
            { o, o, o, o, o },
            { o, o, o, x, o },
            { x, x, x, x, x },
            { o, o, o, x, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix78, 3.92f, "t right"));

        bool[,] comboMatrix79 = new bool[,]
        {
            { o, o, o, o, o },
            { o, o, o, o, x },
            { x, x, x, x, x },
            { o, o, o, o, x },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix79, 3.9f, "T right"));
        
        bool[,] comboMatrix80 = new bool[,]
        {
            { o, o, o, o, o },
            { x, o, x, o, o },
            { o, x, o, o, o },
            { x, o, x, o, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix80, 4.2f, "Diagonal Cross left"));

        bool[,] comboMatrix81 = new bool[,]
        {
            { o, o, o, o, o },
            { o, x, o, x, o },
            { o, o, x, o, o },
            { o, x, o, x, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix81, 4.25f, "Diagonal Cross middle"));

        bool[,] comboMatrix82 = new bool[,]
        {
            { o, o, o, o, o },
            { o, o, x, o, x },
            { o, o, o, x, o },
            { o, o, x, o, x },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix82, 4.15f, "Diagonal Cross right"));

        bool[,] comboMatrix83 = new bool[,]
        {
            { o, o, o, o, o },
            { o, x, o, x, o },
            { x, o, x, o, x },
            { o, x, o, x, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix83, 4.5f, "Infinity"));

        bool[,] comboMatrix84 = new bool[,]
        {
            { o, o, o, o, o },
            { x, o, x, o, x },
            { o, x, o, x, o },
            { x, o, x, o, x },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix84, 4.75f, "Double Cross"));

        bool[,] comboMatrix85 = new bool[,]
        {
            { o, o, o, o, o },
            { x, x, x, o, o },
            { x, x, x, o, o },
            { x, x, x, o, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix85, 5f, "Square Left"));

        bool[,] comboMatrix86 = new bool[,]
        {
            { o, o, o, o, o },
            { o, x, x, x, o },
            { o, x, x, x, o },
            { o, x, x, x, o },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix86, 5.1f, "Square Middle"));

        bool[,] comboMatrix87 = new bool[,]
        {
            { o, o, o, o, o },
            { o, o, x, x, x },
            { o, o, x, x, x },
            { o, o, x, x, x },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix87, 4.9f, "Square Right"));
        
        bool[,] comboMatrix88 = new bool[,]
        {
            { o, o, o, o, o },
            { x, x, x, x, x },
            { o, o, o, o, o },
            { x, x, x, x, x },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix88, 5.5f, "Double Horizontal 5"));

        bool[,] comboMatrix89 = new bool[,]
        {
            { o, o, o, o, o },
            { x, x, x, x, x },
            { o, o, x, o, o },
            { x, x, x, x, x },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix89, 6f, "Wide I"));

        bool[,] comboMatrix90 = new bool[,]
        {
            { o, o, o, o, o },
            { x, x, x, x, x },
            { x, x, x, x, x },
            { x, x, x, x, x },
            { o, o, o, o, o }
        };
        combinations.Add(new Combination(comboMatrix90, 7f, "JACKPOT!"));
    }
}

[System.Serializable]
public class Combination
{
    public bool[,] boolMatrix; // Matrix for combination pattern
    public float comboValue; // Value of the combination
    public string comboName; // Name of the combination

    // Constructor
    public Combination(bool[,] matrix, float value, string name)
    {
        this.boolMatrix = matrix;
        this.comboValue = value;
        this.comboName = name;
    }
}

public static class SlotHandlerExtensions
{
    public static GameObject GetIconInstanceAtPosition(this SlotHandler slotHandler, Vector2 position)
    {
        foreach (var slot in slotHandler.ActiveIcons)
        {
            foreach (var icon in slot)
            {
                if (Vector2.Distance(new Vector2(icon.transform.position.x, icon.transform.position.y), position) < 0.1f)
                {
                    return icon;
                }
            }
        }
        return null;
    }
}
