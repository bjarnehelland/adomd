import React from 'react'
import './App.css'
import Editor from './Editor'
import Table from './Table'

const App: React.FC = () => {
  const [query, setQuery] = React.useState(`SELECT
  {[Measures].[ItemGrossMarginAmt],[Measures].[ItemSalesNetAmt]} ON COLUMNS,
  {[Item].[ItemGrouping].[All Items].children} ON ROWS
  FROM [Retail]`)
  const [data, setData] = React.useState()

  function runQuery() {
    fetch('/api/query')
      .then(res => res.json())
      .then(setData)
  }

  return (
    <div className="App">
      <Editor value={query} onChange={setQuery} />
      <button onClick={runQuery}>Run</button>
      <Table data={data} />
    </div>
  )
}

export default App
