import React from 'react'

type Props = {
  value: string
  onChange: (value: string) => void
}

type TreeItem = {
  value: string
  children: string[]
}
type StepTree = {
  root: string
  steps: { [x: string]: TreeItem }
}
export default function Editor({ value, onChange }: Props) {
  const [steps, setSteps] = React.useState<StepTree>({
    root: 'root',
    steps: {
      root: {
        value: `SELECT <<col>> ON COLUMNS, <<row>> ON ROWS FROM [Retail]`,
        children: ['col', 'row'],
      },
      col: {
        value: '{[Measures].[ItemGrossMarginAmt],[Measures].[ItemSalesNetAmt]}',
        children: [],
      },
      row: {
        value: '{[Item].[ItemGrouping].[All Items].children}',
        children: [],
      },
    },
  })
  function addStep() {
    let val = steps.steps[steps.root].value
    steps.steps[steps.root].children.forEach(child => {
      val = val.replace(`<<${child}>>`, steps.steps[child].value)
    })
    onChange(val)
  }
  return (
    <div>
      <button onClick={addStep}>add</button>
      <textarea
        rows={10}
        style={{ width: '100%', boxSizing: 'border-box' }}
        value={value}
        onChange={e => onChange(e.target.value)}
      />
    </div>
  )
}
