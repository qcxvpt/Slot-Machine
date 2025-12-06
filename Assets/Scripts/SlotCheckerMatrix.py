class Combo:
    def __init__(self, value, matrix):
        self.value = value
        self.matrix = matrix


class SlotMatrixChecker:
    def __init__(self, icon_matrix, combo_array):
        """
        :param icon_matrix: A matrix (2D list) of icons represented by integers
        :param combo_array: A list of Combo objects, each containing a matrix of checkable slots and a value
        """
        self.icon_matrix = icon_matrix
        self.combo_array = combo_array

    def check_combos(self):
        """
        Check each combo's matrix and verify if all icons in the marked columns are the same.
        :return: A dictionary of results for each combo. True if all icons in the marked columns are the same, False otherwise.
        """
        combo_results = {}

        for i, combo in enumerate(self.combo_array):
            results = []
            checkable_slots = combo.matrix

            # Iterate over each column in the combo matrix
            for col in range(len(checkable_slots[0])):
                if checkable_slots[0][col]:  # Check if the column is marked as True
                    # Collect the icons from each row for the current column
                    icons_in_column = [self.icon_matrix[row][col] for row in range(len(self.icon_matrix))]

                    # Check if all icons in the column are the same
                    if all(icon == icons_in_column[0] for icon in icons_in_column):
                        results.append(True)
                    else:
                        results.append(False)
                else:
                    # If the column is not checkable, append None
                    results.append(None)

            combo_results[f"combo_{i+1}"] = results

        return combo_results


# Example data
icon_matrix = [
    [2, 3, 4, 1, 2],
    [2, 1, 4, 2, 2],
    [2, 5, 4, 1, 2]
]

# Combo arrays
combo_array = [
    Combo(0.5, [
        [False, False, False, False, True],  
        [False, False, False, False, True],
        [False, False, False, False, True]
    ]),
    Combo(0.5, [
        [False, False, True, True, True],  
        [False, False, False, False, False],
        [False, False, False, False, False]
    ]),
    Combo(0.75, [
        [True, False, False, True, False],  
        [False, False, True, False, True],
        [True, True, False, False, False]
    ])
]

# Initialize the SlotMatrixChecker
checker = SlotMatrixChecker(icon_matrix, combo_array)

# Perform the check
results = checker.check_combos()

# Display the results
print("Combo Check Results:", results)
