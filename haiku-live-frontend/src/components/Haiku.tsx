import { Haiku, Author } from '../../types'

interface HaikuProps {
  haiku: Haiku
  author: Author
}

export default function HaikuElement({ haiku, author }: HaikuProps) {
  return (
    <div className='haiku-container bg-amber-200/30 rounded-md p-4 m-2'>
      <p className='haiku'>
        <span className='haiku-line block'>
          {haiku.line1}
        </span>

        <span className='haiku-line block'>
          {haiku.line2}
        </span>

        <span className='haiku-line block'>
          {haiku.line3}
        </span>
      </p>

      <p className='author-line text-right text-sm font-semibold'>
        {haiku.authorName}
      </p>
    </div>
  )
}
