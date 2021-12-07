import pandas as pd

types_df = pd.read_csv('types.csv')

print(types_df)

effective = {}
weak = {}
no_effect = {}

# Attacks
for ind, row in types_df.iterrows():
    row_effectives = []
    row_weaks = []
    row_immunities = []

    row_type = row[0]

    # Get all the weakness and resistances
    for i in range(1, len(row)):

        if row[i] == 2:
            row_effectives.append(types_df.columns[i])
        if row[i] == 0.5:
            row_weaks.append(types_df.columns[i])
        if row[i] == 0:
            row_immunities.append(types_df.columns[i])
    
    effective[row_type] = row_effectives
    weak[row_type] = row_weaks
    no_effect[row_type] = row_immunities


print(effective)

with open('attackMatchups.csv', 'w') as file:

    for e, w, i in zip(effective, weak, no_effect):
        file.write("\n***\n")
        file.write(e+'\n')

        file.write('Effective\n')
        for index, type in enumerate(effective[e]):
            file.write(type)
            if index != len(effective[e]) - 1:
                file.write(',')

        file.write('\nWeak\n')
        for index, type in enumerate(weak[e]):
            file.write(type)
            if index != len(weak[e]) - 1:
                file.write(',')
        
        file.write('\nImmune\n')
        for index, type in enumerate(no_effect[e]):
            file.write(type)
            if index != len(no_effect[e]) - 1:
                file.write(',')
    