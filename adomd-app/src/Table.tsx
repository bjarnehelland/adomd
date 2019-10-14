import React from 'react'

type Props = {
  data?: any[]
}

export default function Table({ data }: Props) {
  if (!data) {
    return <div>No data</div>
  }
  return (
    <div>
      <table>
        <tbody>
          {data.map((row, r) => (
            <tr key={r}>
              {Object.keys(row).map(cellKey => (
                <td key={cellKey}>{row[cellKey]}</td>
              ))}
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  )
}
