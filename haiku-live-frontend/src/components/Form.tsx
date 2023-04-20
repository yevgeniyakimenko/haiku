import { NewHaiku } from '../../types'

interface FormProps {
  authorId: number
  authorName: string
  topicId: number
  onSubmit: (haiku: NewHaiku) => void
}

export default function Form({ authorId, authorName, topicId, onSubmit }: FormProps) {
  function handleSubmit(e: React.FormEvent<HTMLFormElement>): void {
    e.preventDefault()
    const elements = e.currentTarget.elements
    const line1 = elements.namedItem('line1') as HTMLInputElement
    const line2 = elements.namedItem('line2') as HTMLInputElement
    const line3 = elements.namedItem('line3') as HTMLInputElement

    const haiku = {
      line1: line1.value,
      line2: line2.value,
      line3: line3.value,
      authorId,
      authorName,
      topicId,
    }

    onSubmit(haiku)
    line1.value = ''
    line2.value = ''
    line3.value = ''
  }
  
  return (
    <div className='form-container w-full'>
      <form onSubmit={handleSubmit}>
        <div className='FormContents flex flex-col items-center'>
          <input 
            autoFocus 
            required
            maxLength={83}
            placeholder={`First line`}
            type="text" 
            id="line1" 
            name="line1" 
            className='focus:outline-none grow border rounded-md bg-amber-100/60 border-amber-900 placeholder:text-zinc-500 w-full px-4 py-2 mb-2' 
          />

          <input 
            required
            maxLength={83}
            placeholder={`Second line`}
            type="text" 
            id="line2" 
            name="line2" 
            className='focus:outline-none grow border rounded-md bg-amber-100/60 border-amber-900 placeholder:text-zinc-500 w-full px-4 py-2 mb-2' 
          />

          <input 
            required
            maxLength={83}
            placeholder={`Third line`}
            type="text" 
            id="line3" 
            name="line3" 
            className='focus:outline-none grow border rounded-md bg-amber-100/60 border-amber-900 placeholder:text-zinc-500 w-full px-4 py-2 mb-2' 
          />

          <button 
            type="submit" className='bg-orange-400 hover:bg-orange-500 border rounded-md border-slate-500 px-3 py-2'
          >
            Send
          </button>
        </div>
      </form>
    </div>
  )
}
